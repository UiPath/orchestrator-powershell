<#
    .SYNOPSIS
        Synchronises Orchestrator users with Windows Active Directory, based on AD group membership mapped to Orchestrator Roles.
    .DESCRIPTION
        You provide the AD domain name and a mapping from relevand AD groups to Orchestrator Roles.
        New users in AD are added to Orchestrator and existing users added moved to the correct Role.
        The script also handles removing Orchestrator users from roles when they were removed from the corresponding AD group.
        AD users that were removed from all relevant AD groups (eg. an employee that changed role) or were removed from AD (eg. a former employee that left the company) become 'orphaned users'. They are still defined in Orchestrator but do not have any Role. The script supports the -OrphanedUsersAction parameter allowing to optionally List or Remove these users.
        The script is idempotent, repeated invocations should not modify the Orchestrator users unless something changed in AD.
        You should first import the UiPath.PowerShell module and authenticate yourself with your Orchestrator using Get-UiPathAuthToken before running this script.
    .PARAMETER DomainName
        The domain to sync users with. It does not necessarily has to be your current user or machine domain, but there must be some trust relationship so your Windows session can discover and interogate this domain AD.
    .PARAMETER RolesMapping
        A Hashtable mapping AD groups to Orchestrator roles. Make sure you type the names correctly.
    .PARAMETER OrphanedUsersAction
        Optional action to handle orphaned users. You can List or Remove these users.
    .EXAMPLE
        Sync-UiPathADUsers MyDomain @{'RPA Admins' = 'Administrator'; 'RPA Users' = 'User'}
        Import AD users from MyDomain and maps the members of the 'RPA Admins' AD group to the 'Administrator' Orchestrator role and members of the 'RPA Users' AD group to the 'User' Orchestrator role.
    .EXAMPLE
        Sync-UiPathADUsers MyDomain @{} -OrphanedUsersAction Remove
        Import AD users from MyDomain but since there is no mapping, the effect is to orphan all exiting Orchestrator MyDomain users and then remove them because of the -OrphanedUsersAction Remove parameter. In effect this invocation removes all MyDomain users from Orchestrator.
#>
param(
    [Parameter(Mandatory=$true, Position=0)] 
    [string] $DomainName,
    [Parameter(Mandatory=$true, Position=1)] 
    [HashTable] $RolesMapping,
    [Parameter(Mandatory=$false)]
    [ValidateSet('Remove', 'List', 'Ignore')]
    [string] $OrphanedUsersAction = 'Ignore'
)

$ErrorActionPreference = "Stop"

function Get-ADGroupUser {
    param(
    [Parameter(Mandatory=$true, Position=0)] $dc,
    [Parameter(Mandatory=$true, Position=1)] $adGroup
    )

    Write-Verbose "Get-ADGroupUser $adGroup"

    $users = @()
    $members = Get-ADGroupMember -Server $dc.PDCEmulator -Identity $adGroup

    foreach($member in $members)
    {
        if ($member.objectClass -eq 'user')
        {
            $users += @($member.SamAccountName)
        }
        elseif ($member.objectClass -eq 'group')
        {
            $childUsers = Get-ADGroupUser $dc $member.SamAccountName
            $users += $childUsers
        }
    }
    $users
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
        if ($role -eq $null)
        {
            throw "Could not find the Orchestrator role name: $roleName"
        }
        Write-Verbose "Role ok: $roleName $($role.Name)"
    }

    Write-Verbose "Get-ADDomain $DomainName"
    $dc = Get-ADDomain -Identity $DomainName

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

        $adGroupMembers = Get-ADGroupUser $dc $adGoupName | sort -Unique

        foreach($adGroupMember in $adGroupMembers)
        {
            $userName = $DomainName + '\' + $adGroupMember
            $adUser = $allADUsers[$userName]
            if ($adUser -eq $null)
            {
                $adUser = @{ roles = @(); name = $adGroupMember.Name }
                $null = $allADUsers.Add($userName, $adUser)
            }
            Write-Verbose "Discovered AD user $userName with role $mappedRole"
            $adUser.roles += @($mappedRole)
        }
    }

    $idxOperationStep += 1
    Write-Progress -Activity "Sync Orchestrator AD Users" `
        -CurrentOperation $operationSteps[$idxOperationStep] `
        -PercentComplete ($idxOperationStep/$operationSteps.Count*100)
    $orchestratorUsers = Get-UiPathUser -Type User | Select UserName, Id, RolesList | where {$_.UserName.StartsWith($DomainName + '\', [System.StringComparison]::OrdinalIgnoreCase)}

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
        if ($op -eq $null)
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
            $newUsers += @{userName = $adUserName; name = $op.adUser.name; roles = $op.adUser.roles}
        }
        else
        {
            $idxExisting = 0
            $idxNew = 0

            $existingRoles = $op.existingRoles | sort -Unique
            $newRoles = $op.newRoles | sort -Unique

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
                elseif ($newRole -eq $null -or (($existingRole -ne $null) -and ($existingRole -lt $newRole)))
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
                if ($changedRole -eq $null)
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

    $i = 0
    foreach($newUser in $newUsers)
    {
        $i += 1

        Write-Progress -Id 1 `
            -Activity $operationSteps[$idxOperationStep] `
            -CurrentOperation $newUser.userName `
            -PercentComplete ($i/$newUsers.Count * 100)
        
        Write-Verbose "Add-UiPathUser -Username $newUser.userName -Name $($newUser.name) -RolesList $($newUser.roles)"
        $null = Add-UiPathUser -Username $newUser.userName -Name $newUser.name -RolesList $newUser.roles
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
                Write-Host $orphanedUser.userName
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
    $script = $_.InvocationInfo.ScriptName
    $msg = $e.Message 

    Write-Error "$klass $msg ($script $line)"
}






