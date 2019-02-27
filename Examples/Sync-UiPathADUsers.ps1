<#
    .SYNOPSIS
        Synchronises Orchestrator users with Windows or Azure Active Directory, based on AD group membership mapped to Orchestrator Roles.
    .DESCRIPTION
        New users in AD are added to Orchestrator and existing users added moved to the correct Role.
        Azure AD users are matched by comparing the Azure AD user principal name with the user Email in Orchestrator.
        The script also handles removing Orchestrator users from roles when they were removed from the corresponding AD group.
        AD users that were removed from all relevant AD groups (eg. an employee that changed role) or were removed from AD (eg. a former employee that left the company) become 'orphaned users'. They are still defined in Orchestrator but do not have any Role. The script supports the -OrphanedUsersAction parameter allowing to optionally List or Remove these users.
        The script is idempotent, repeated invocations should not modify the Orchestrator users unless something changed in AD.
        You should first import the UiPath.PowerShell module and authenticate yourself with your Orchestrator using Get-UiPathAuthToken before running this script.
        The script does not modify the Admin user roles membership, even if the Email matches the AzureAD domains. This is a common scenario and can result in accidentally locking Admin user out of Administrators group.
        The script adds new Orchestrator users using the Azure AD DisplayName as Name and leaves Surname empty. It does not try to split the DisplayName and figure out the Surname.
    .PARAMETER DomainName
        The Windows domain to sync users with. It does not necessarily has to be your current user or machine domain, but there must be some trust relationship so your Windows session can discover and interogate this domain AD.
    .PARAMETER AzureAD
        Use currently connected Azure AD for sync. You must first connect the PowerShell session to Azure AD using Connect-AzureAD
    .PARAMETER RolesMapping
        A Hashtable mapping AD groups to Orchestrator roles. Make sure you type the names correctly.
    .PARAMETER OrphanedUsersAction
        Optional action to handle orphaned users. You can List or Remove these users.
    .PARAMETER AllowUsernameTruncate
        Optional switch to allow truncation of imported usernames to 32 characters, the Orchestrator username length limit.
    .EXAMPLE
        Sync-UiPathADUsers MyDomain @{'RPA Admins' = 'Administrator'; 'RPA Users' = 'Users'}
        Import AD users from MyDomain and maps the members of the 'RPA Admins' AD group to the 'Administrator' Orchestrator role and members of the 'RPA Users' AD group to the 'Users' Orchestrator role.
    .EXAMPLE
        Sync-UiPathADUsers -AzureAD @{'RPA Admins' = 'Administrator'; 'RPA Users' = 'Users'}
        Import AD users from Azure Active Directory and maps the members of the 'RPA Admins' Azure AD group to the 'Administrator' Orchestrator role and members of the 'RPA Users' Azure AD group to the 'Users' Orchestrator role.
    .EXAMPLE
        Sync-UiPathADUsers MyDomain @{} -OrphanedUsersAction Remove
        Import AD users from MyDomain but since there is no mapping, the effect is to orphan all exiting Orchestrator MyDomain users and then remove them because of the -OrphanedUsersAction Remove parameter. In effect this invocation removes all MyDomain users from Orchestrator.
    .EXAMPLE
        Sync-UiPathADUsers -AzureAD @{} -OrphanedUsersAction Remove
        Deletes all Azure Active Directory managed users from Orchestrator.
        Important notice: this will remove any user in Orchestrator that has an Email domain matching the Azure AD domain, even if it was not imported from Azure AD. 
#>
param(
    [Parameter(Mandatory=$true, Position=0, ParameterSetName="Windows")] 
    [string] $DomainName,
    [Parameter(Mandatory=$true, Position=0, ParameterSetName="Azure")] 
    [switch] $AzureAD,
    [Parameter(Mandatory=$true, Position=1)] 
    [HashTable] $RolesMapping,
    [Parameter(Mandatory=$false)]
    [ValidateSet('Remove', 'List', 'Ignore')]
    [string] $OrphanedUsersAction = 'Ignore',
    [Parameter()]
    [switch] $AllowUsernameTruncate,
    [Parameter()]
    [string[]] $AllowedDomainNames = @()
    )

$ErrorActionPreference = "Stop"

$isAzureAD = $PSCmdlet.ParameterSetName -eq "Azure"
$isWindows = $PSCmdlet.ParameterSetName -eq "Windows"

function Get-IsAzureAD {
    return $isAzureAD
}

function Get-IsWindows {
    return $isWindows
}

function Get-DomainName{
    param(
        [Parameter(Position=0, Mandatory=$True)] $distinguishedName
    )
    
    $dcs = $distinguishedName -split ','| Select-String -Pattern "DC=" | ForEach-Object{ $_ -replace 'DC=', ''}
    $extractedDomainName = $dcs -join '.'
    Write-Verbose "extracted domain: $extractedDomainName  from distinguishedName: $distinguishedName extracted as $extractedDomainName"
    return $dcs -join '.'
}

function Format-UiPathUserName {
    param(
        [Parameter(Position=0, Mandatory=$True)] $userName,
        [Parameter(Position=1, Mandatory=$False)] $adNetBiosName = $null
    )

    if (-not [string]::IsNullOrWhiteSpace($adNetBiosName)) {
        $userName = $adNetBiosName + '\' + $userName
    }
  
    if ($userName.Length -gt 32) {
        if ($AllowUsernameTruncate) {
            Write-Warning "AD Username $username exceeds the 32 character limit."
            $userName = $userName.Substring(0,32)
        } 
        else {
            throw "AD Username $username exceeds the 32 character limit. Run the script with -AllowUsernameTruncate to force user with a truncated name creation."
        }
    }
    return $userName
}

function Get-ADGroupUser {
    param(
    [Parameter(Position=0)] $groupDomainName,
    [Parameter(Position=1)] $adGroup
    )

    $users = @()
    Write-Verbose "Get-ADGroupUser $adGroup"

    if (Get-IsWindows) {


        Write-Progress -Id 1 `
            -Activity "Retrieve group members: $groupDomainName" `

        Write-Verbose "Get-ADGroupMember -Server $($groupDomainName) -Identity $adGroup"
        $members = @(Get-ADGroupMember -Server $groupDomainName -Identity $adGroup)

        foreach($member in $members)
        {
            $currentDomainName = Get-DomainName $member.DistinguishedName

            if(-not $knownDomains.ContainsKey($currentDomainName)){
                continue
            }

            $dc = $knownDomains[$currentDomainName]

            if ($member.objectClass -eq 'user')
            {
                $userInfo = @{
                    SamAccountName = $member.SamAccountName;
                    Username = Format-UiPathUserName $member.SamAccountName $dc.NetBIOSName;
                    DNSRoot = $currentDomainName
                }

                $users += $userInfo
            }
            elseif ($member.objectClass -eq 'group')
            {
                $childUsers = Get-ADGroupUser $currentDomainName $member.SamAccountName
                $users += $childUsers
            }
        }
    }

    if (Get-IsAzureAD) {
        $adGroupObject = Get-AzureADGroup -SearchString $adGroup | Where-Object {$_.DisplayName -eq $adGroup}
        Write-Verbose "Get-AzureADGroupMember $($adGroupObject.ObjectId)"
        $members = Get-AzureADGroupMember -ObjectId $adGroupObject.ObjectId

        $users += $members | ForEach-Object {@{
            UPN = $_.UserPrincipalName;
            Username = Format-UiPathUserName $_.UserPrincipalName;
            Name=$_.DisplayName; 
        }}
    }

    $users | ForEach-Object {Write-Verbose "$adGroup AD group user for sync: $($_.UPN) Username:$($_.Username) Name:$($_.Name)"}

    $users
}

function Get-UiPathADUsers {
    param(
    [Parameter(Position=0)] $domain)

    Write-Verbose "Get-UiPathUser -Type User "
    $allUsers = Get-UiPathUser -Type User | Where-Object {$_.Username -ne "Admin"}
    $OrchestratorADUsers = @()
    $conditions = $null
    if (Get-IsWindows) {
        $dcNetBiosNames =  $knownDomains.Values | ForEach-Object {$_.NetBIOSName}
        $conditions = {$dcNetBiosNames -contains $_.UserName.Split('\')[0]}
    }

    if (Get-IsAzureAD) {
        # Orchestrator maps user to Azure AD identities based on Orchestrator user Email address
        Write-Verbose "Get-AzureADDomain"
        $domains = Get-AzureADDomain | ForEach-Object {$_.Name}
        $domains | ForEach-Object { Write-Verbose "AzureAD Email domain: $_"}
        $conditions = {$domains -contains $_.EmailAddress.Split('@')[1]}
    }
    $OrchestratorADUsers += $allUsers | Where-Object $conditions | Select-Object UserName, Id, RolesList, EmailAddress
    $OrchestratorADUsers | ForEach-Object {Write-Verbose "Orchestrator user for sync: $($_.Id) $($_.Username) $($_.EmailAddress)"}

    return $OrchestratorADUsers
}

try
{
    $operationSteps = @(
        "Validate Orchestrator role names", `
        "Extracting AD group members", `
        "Extracting Orchestrator users", `
        "Preparing sync operations", `
        "Importing New AD Users", `
        "Modifying user role membership", `
        "Process orphaned users"
    )

    $idxOperationStep = 0
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)
    $i = 1
    foreach($roleName in $RolesMapping.Values)
    {
        Write-Progress -Id 1 `
            -Activity $operationSteps[$idxOperationStep] `
            -CurrentOperation $roleName `
            -PercentComplete ($i/$RolesMapping.Values.Count*100)
        $i += 1
        $role = Get-UiPathRole -Name $roleName
        if ($null -eq $role)
        {
            throw "Could not find the Orchestrator role name: $roleName"
        }
        Write-Verbose "Role ok: $roleName $($role.Name)"
    }

    $knownDomains = @{}
    $mainDc = $null

    if (Get-IsWindows) {
        Write-Verbose "Get-ADDomain $DomainName"
        $dc = Get-ADDomain -Identity $DomainName
        $knownDomains.Add($dc.DNSRoot,$dc)
        $mainDc = $dc;
        foreach($dn in $AllowedDomainNames){
            Write-Verbose "Get-ADDomain $dn"
            $dc = Get-ADDomain -Identity $dn
            $knownDomains.Add($dc.DNSRoot,$dc)
        }
    }

    $idxOperationStep += 1
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)

    [HashTable] $allADUsers = @{}

    $i = 1
    foreach($adGoupName in $RolesMapping.Keys)
    {
        Write-Progress -Id 1 `
            -Activity $operationSteps[$idxOperationStep] `
            -PercentComplete ($i/$RolesMapping.Keys.Count *100) `
            -CurrentOperation $roleMap
        $i += 1

        $mappedRole = $RolesMapping[$adGoupName]

        $adGroupMembers = Get-ADGroupUser $DomainName $adGoupName | Sort-Object {$_.Username} -Unique

        foreach($adGroupMember in $adGroupMembers)
        {
            $userName = $adGroupMember.Username 
            $adUser = $allADUsers[$userName]
            if ($null -eq $adUser)
            {
                $adUser = @{ roles = @(); userInfo = $adGroupMember}
                $null = $allADUsers.Add($userName, $adUser)
            }
            $adUser.roles += @($mappedRole)
        }
    }

    $idxOperationStep += 1
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)
    $orchestratorUsers = Get-UiPathADUsers $DomainName

    $idxOperationStep += 1
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)

    [HashTable] $operations = @{}

    # Copy all Orchestrator users 
    foreach($orchestratorUser in $orchestratorUsers)
    {
        $op = @{id = $orchestratorUser.Id; isNew = $false; existingRoles = $orchestratorUser.RolesList; newRoles = @()}
        $null = $operations.Add($orchestratorUser.UserName, $op)
    }

    # Apply all AD users
    foreach($adUserName in $allADUsers.Keys)
    {
        $op =  $operations[$adUserName]
        $adUser = $allADUsers[$adUserName]
        if ($null -eq $op)
        {
            $op = @{isNew = $true; adUser = $adUser}
            $operations.Add($adUserName, $op)
        }
        else
        {
            $op.newRoles +=  $adUser.roles
        }
    }


    # Figure add/remove list for each group and the new users that need to be added

    $newUsers = @()
    $orphanedUsers = @()
    $changedRoles = @{}
    foreach($adUserName in $operations.Keys)
    {
        $op = $operations[$adUserName]

        if ($op.isNew -eq $true)
        {
            $newUsers += @{userName = $adUserName; userInfo = $op.adUser.userInfo; roles = $op.adUser.roles; }
        }
        else
        {
            $idxExisting = 0
            $idxNew = 0

            $existingRoles = $op.existingRoles | Sort-Object -Unique
            $newRoles = $op.newRoles | Sort-Object -Unique

            # Oh, Powershell....
            if ($existingRoles -isnot [array])
            {
                $existingRoles = @($existingRoles)
            }

            if ($newRoles -isnot [array])
            {
                $newRoles = @($newRoles)
            }

            # because we sorted the two arrays we can use a merge algorithm

            while($idxExisting -lt $existingRoles.Count -or $idxNew -lt $newRoles.Count)
            {
                $existingRole = $null
                $newRole = $null
                $changedRole = $null
                $addOrRemove = $null
                $roleName = $null

                if ($idxExisting -lt $existingRoles.Count)
                {
                    $existingRole = $existingRoles[$idxExisting]
                }

                if ($idxNew -lt $newRoles.Count)
                {
                    $newRole = $newRoles[$idxNew]
                }

                if ($existingRole -eq $newRole)
                {
                    # unchanged role, nothing to see here, move along
                    $idxExisting += 1
                    $idxNew += 1
                    continue
                }
                elseif ($null -eq $newRole -or (($null -ne $existingRole) -and ($existingRole -lt $newRole)))
                {
                    # user must be removed from this existing role
                    $roleName = $existingRole
                    $idxExisting += 1
                    $addOrRemove = 'remove'
                }
                else
                {
                    # user must be added to this new role
                    $roleName = $newRole
                    $idxNew += 1
                    $addOrRemove = 'add'
                }
                $changedRole = $changedRoles[$roleName]
                if ($null -eq $changedRole)
                {
                    $changedRole = @{addedUsers=@();removedUsers=@()}
                    $changedRoles.Add($roleName, $changedRole)
                }
                if ($addOrRemove -eq 'add')
                {
                    $changedRole.addedUsers += @($op.Id)
                }
                else
                {
                    $changedRole.removedUsers += @($op.Id)
                }
            }

            if ($newRoles.Count -eq 0)
            {
                Write-Verbose "Is orphaned: $adUserName"
                $orphanedUsers += @{userName = $adUserName; id = $op.Id}
            }
        }
    }

    $idxOperationStep += 1
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)
    
    $ouId = $null
    $token = Get-UiPathAuthToken -CurrentSession
    if (-not [string]::IsNullOrWhiteSpace($token.OrganizationUnit)) {
      Write-Verbose "Get-UiPathOrganizationUnit - DisplayName $($token.OrganizationUnit)"
      $ouId = (Get-UiPathOrganizationUnit -DisplayName $token.OrganizationUnit).Id
    }

    $i = 0
    foreach($newUser in $newUsers)
    {
        $i += 1

        Write-Progress -Id 1 `
            -Activity $operationSteps[$idxOperationStep] `
            -CurrentOperation $newUser.userName `
            -PercentComplete ($i/$newUsers.Count * 100)
        try {
            $userDomainName = $DomainName
            if (Get-IsWindows) {
                $userDomainName = $mainDc.DNSRoot
                # We postponed getting the details until actually needed to reduce AD chit-chat
                Write-Verbose "Get-ADUser -Server $($newUser.userInfo.DNSRoot) -Identity $($newUser.userInfo.SamAccountName) -Properties EmailAddress"
                $adUserInfo = Get-ADUser -Server $newUser.userInfo.DNSRoot -Identity $newUser.userInfo.SamAccountName -Properties EmailAddress
    
                if (-not [string]::IsNullOrWhiteSpace($adUserInfo.EmailAddress)) {
                    $newUser.userInfo.EmailAddress = $adUserInfo.EmailAddress
                }
                elseif (-not [string]::IsNullOrWhiteSpace($adUserInfo.UserPrincipalName)) {
                    $newUser.userInfo.EmailAddress = $adUserInfo.UserPrincipalName
                }
            }
    
            $cmdMsg = "Add-UiPathUser -Username $($newUser.userName) -RolesList ..."
            $cmd = 'Add-UiPathUser -Username $newUser.userName -RolesList $newUser.roles'
            
            if (Get-IsWindows) {
              $cmdMsg += " -Domain $($userDomainName)"
              $cmd += ' -Domain $userDomainName'
            }else{
                if (-not [string]::IsNullOrWhiteSpace($newUser.userInfo.name)) {
                    $cmdMsg += " -Name $($newUser.userInfo.name)"
                    $cmd += ' -Name $newUser.userInfo.name'
                }
        
                if (-not [string]::IsNullOrWhiteSpace($newUser.userInfo.Surname)) {
                    $cmdMsg += " -Surname $($newUser.userInfo.Surname)"
                    $cmd += ' -Surname $newUser.userInfo.Surname'
                }
            }
    
            if (-not [string]::IsNullOrWhiteSpace($newUser.userInfo.EmailAddress)) {
                $cmdMsg += " -EmailAddress $($newUser.userInfo.EmailAddress)"
                $cmd += ' -EmailAddress $newUser.userInfo.EmailAddress'
            } elseif (-not [string]::IsNullOrWhiteSpace($newUser.userInfo.UPN)) {
                $cmdMsg += " -EmailAddress $($newUser.userInfo.UPN)"
                $cmd += ' -EmailAddress $newUser.userInfo.UPN'
            }
            
            if ($null -ne $ouId) {
              $cmdMsg += " -OrganizationUnitIds @($ouId)"
              $cmd += ' -OrganizationUnitIds @($ouId)'
            }
            
            Write-Verbose $cmdMsg
            $null = Invoke-Expression $cmd
        }
        catch {
            $e = $_.Exception
            $line = $_.InvocationInfo.ScriptLineNumber
            $msg = $e.Message 

            Write-Warning "$($line): $msg"
        }
    }

    $idxOperationStep += 1
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)

    $i = 0
    foreach($changedRole in $changedRoles.Keys)
    {
        $i += 1
        $op = $changedRoles[$changedRole]

        Write-Progress -Id 1 `
            -Activity $operationSteps[$idxOperationStep] `
            -CurrentOperation $changedRole `
            -PercentComplete ($i/$changedRoles.Keys.Count * 100)

        Write-Verbose "Get-UiPathRole -Name $changedRole"
        $role = Get-UiPathRole -Name $changedRole
        
        Write-Verbose "Edit-UiPathRoleUser $changedRole -Add $($op.addedUsers) -Remove $($op.removedUsers)"
        $null = Edit-UiPathRoleUser $role -Add $op.addedUsers -Remove $op.removedUsers
    }

    $idxOperationStep += 1
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)

    switch($OrphanedUsersAction)
    {
        'Remove' 
        {
            $i = 1
            foreach($orphanedUser in $orphanedUsers)
            {
                Write-Progress -Id 1 `
                    -Activity "Remove orphaned users" `
                    -CurrentOperation $orphanedUser.userName `
                    -PercentComplete ($i/$orphanedUsers.Count * 100)
                $i += 1
                Write-Verbose "Remove-UiPathUser -Id $($orphanedUser.id)"
                Remove-UiPathUser -Id $orphanedUser.id
            }
        }
        'List' 
        {
            foreach($orphanedUser in $orphanedUsers)
            {
                New-Object PSObject | 
                    Add-Member Username $orphanedUser.userName -PassThru |
                    Add-Member Id $orphanedUser.Id -PassThru |
                    Write-Output
            }
        }
        'Ignore' 
        {
            # No-op
        }
    }
}
catch
{
    $e = $_.Exception
    $klass = $e.GetType().Name
    $line = $_.InvocationInfo.ScriptLineNumber
    $msg = $e.Message 

    Write-Error "$($line): $($klass): $msg"
}






