<###########################################################

README FIRTS

Tested with
    - PowerShell 5.1.18362.628 
    - Orchestrator 2019.10.17 - from one tenant to another of the same instance
    - UiPath.PowerShell 20.2.0.13

Before you run
    - It is advised to perform a back-up of your current Orchestrator DB
    - The script was built using the Orchestrator PowerShell cmdlet library: https://github.com/UiPath/orchestrator-powershell
    - At the time of writing, there are some components not supported by the library, so direct HTTP Requests were required
    - The script is not intended to be a silver bullet, PowerShell knowledge to adapt it is required
    - The script will verify if a specific component already exists on the destination tenant, and if so it will skip it
    - Some edits are required before you run:
        - Source and destination tenant configurations. 
        - Actual execution steps are at the bottom. Select the required ones

Limitations
    - Modern Folders are not supported
    - Only local Orchestrator Users are supported
    - Tenant settings migration is not supported, except for Non-Working-Days Calendars
    - Task Catalogs migration is not implemented
    - License migration is not implemented
    - Passwords of Robots, Users and Credentials are not migrated - you will have to fill them in manually afterwards
    - Destination storage location for packages and libraries might + the actual path of the nupkgs might exceed the Windows FilePath Max Length
    
Suggested Improvements
    - When adding elements, instead of checking for each of them, we could interogate the Destination Tenant for all existing components, and then only try to add the missing ones

############################################################>

############# EDIT the following section #############
$SourceTenantConfig = [PSCustomObject]@{
    'url'   = "https://orchestrator.url/"
    'tenant' = "default"
    'username' = "admin"
    'password' = "****************" # TODO - Change this accordingly
    'packagesPath' = "C:\Program Files (x86)\UiPath\Orchestrator\Storage\Orchestrator-9aacf2f5-3894-40c3-a879-3736919abf75\Processes"
    'librariesPath' = "C:\Program Files (x86)\UiPath\Orchestrator\Storage\Orchestrator-Host\Libraries"
    'tokenExpirationInMinutes' =  30
}

$DestinationTenantConfig = [PSCustomObject]@{
    'url'   = "https://orchestrator.url/"
    'tenant' = "default"
    'username' = "admin"
    'password' = "****************" # TODO - Change this accordingly
    'packagesPath' = "C:\Program Files (x86)\UiPath\Orchestrator\Storage\Orchestrator-5c8fb6ac-19fc-4e25-adf2-697b16ac4e8c\Processes"
    'librariesPath' = "C:\Program Files (x86)\UiPath\Orchestrator\Storage\Orchestrator-Host\Libraries"
    'DefaultPassword' = "****************"   # TODO - Change this accordingly - will be used when creating Credential Assets and Robot passwords and will have to be modified later manually
    'tokenExpirationInMinutes' = 30
}

# You can Disable Debug logs by commenting the next line
$DebugPreference = 'Continue'

######################################################

############# Requirements #############
$supportedOrchestratorVersions = ("2019.10")
$requiredUipathPowerShellVersion = [System.Version]"20.2.0.13"

if ((Get-Module -Name "UiPath.PowerShell") -eq $null)
{
    Write-Error "UiPath.PowerShell is not installed. Please install it following instructions here: https://github.com/UiPath/orchestrator-powershell"
    Exit
}
elseif ((Get-Module -Name "UiPath.PowerShell").Version -lt $requiredUipathPowerShellVersion)
{
    Write-Error "Minimum required UiPath.PowerShell version $($requiredUipathPowerShellVersion) not found on this machine. Execution will stop"
    Exit
}

Import-Module UiPath.PowerShell

########################################


function GetTenantStructure
{
    Param([PSObject] $tenantConfig)

    Write-Host "Getting Tenant Structure from $($tenantConfig.url) $($tenantConfig.tenant)"

    $authResponse = Get-UiPathAuthToken -URL $tenantConfig.url -TenantName $tenantConfig.tenant -Username $tenantConfig.username -Password $tenantConfig.password -Session
    
    # Calendars are not yet supported in UiPath.PowerShell
    $response = Invoke-WebRequest -Uri ($tenantConfig.url + "/odata/Calendars") -Method Get -ContentType "application/json" -Headers @{"Authorization"="Bearer " + $authResponse.Token}
    $calendars = (ConvertFrom-Json $response.Content).value
    for($i=0; $i -lt $calendars.Count; $i++)
    {
        $response = Invoke-WebRequest -Uri ($tenantConfig.url + "/odata/Calendars("+$calendars[$i].Id +")") -Method Get -ContentType "application/json" -Headers @{"Authorization"="Bearer " + $authResponse.Token}
        $calendars[$i] = (ConvertFrom-Json $response.Content)
        $calendars[$i].PSObject.Properties.Remove('@odata.context')
    }
    Write-Debug "Found calendars: $($calendars.Count)"

    $machines = Get-UiPathMachine
    Write-Debug "Found machines: $($machines.Count)"
    $packages = Get-UiPathPackage | Get-UiPathPackageVersion
    Write-Debug "Found packages: $($packages.Count)"
    $libraries = Get-UiPathLibrary | Get-UiPathLibraryVersion
    Write-Debug "Found libraries: $($libraries.Count)"
    $roles = Get-UiPathRole
    Write-Debug "Found roles: $($roles.Count)"
    $users = Get-UiPathUser | Where-Object {$_.Type -eq 0} # Only retrieve the local users. AD Users should be imported from the AD
    Write-Debug "Found users: $($users.Count)"
    $folders = Get-UiPathFolder | Where-Object {$_.ProvisionType -eq "Manual"} # Only supports Classic Folders
    Write-Debug "Found folders: $($folders.Count)"
    
    foreach ($folder in $folders)
    {
        $authResponse = Get-UiPathAuthToken -URL $tenantConfig.url -TenantName $tenantConfig.tenant -Username $tenantConfig.username -Password $tenantConfig.password -Session -OrganizationUnit $folder.FullyQualifiedName
        
        $usersStructure = New-Object System.Collections.ArrayList
        $folderUsers = Get-UiPathFolderUsers -Id $folder.Id
        if ($folderUsers.Count -eq 1)
        {
            $usersStructure.Add($folderUsers) 1>$null
        }
        elseif($folderUsers.Count -gt 1)
        {
            $usersStructure.AddRange($folderUsers) 1>$null
        }
        Write-Debug "<$($folder.FullyQualifiedName)> Found folderUsers: $($folderUsers.Count)"

        $queues = Get-UiPathQueueDefinition
        Write-Debug "<$($folder.FullyQualifiedName)> Found queues: $($queues.Count)"
        $processes = Get-UiPathProcess
        Write-Debug "<$($folder.FullyQualifiedName)> Found processes: $($processes.Count)"
        $robots = Get-UiPathRobot
        Write-Debug "<$($folder.FullyQualifiedName)> Found robots: $($robots.Count)"

        # Triggers are not yet supported in UiPath.PowerShell
        $response = Invoke-WebRequest -Uri ($tenantConfig.url + "/odata/ProcessSchedules") -Method Get -ContentType "application/json" -Headers @{"Authorization"="Bearer " + $authResponse.Token; "X-UIPATH-OrganizationUnitId"=$folder.Id}
        $triggers = (ConvertFrom-Json $response.Content).value
        Write-Debug "<$($folder.FullyQualifiedName)> Found triggers: $($triggers.Count)"

        $assets = Get-UiPathAsset
        # Workaround: to make the structure serializable we need to remove the Dictionaries
        for($i=0; $i -lt $assets.Count; $i++)
        {
            
            $robotValues = $assets[$i].RobotValues 
            $keyValueList = $assets[$i].KeyValueList

            $assets[$i] = $assets[$i] | Select-Object -Property * -ExcludeProperty RobotValues
            $assets[$i] = $assets[$i] | Select-Object -Property * -ExcludeProperty KeyValueList

            $newRobotValues = New-Object System.Collections.ArrayList
            if ($robotValues.Keys)
            {
                foreach($key in $robotValues.Keys)
                {
                    $robotObject = $robots | Where-Object {$_.Id -eq $key}
                    $newRobotValues.Add(@{Id=$key; Name=$robotObject.Name; Value=$robotValues[$key]}) > $null
                }
                
                $assets[$i] | Add-Member -NotePropertyName RobotValues -NotePropertyValue $newRobotValues -Force
            }
        }
        Write-Debug "<$($folder.FullyQualifiedName)> Found assets: $($assets.Count)"

        $environments = Get-UiPathEnvironment
        # Workaround to include the Robots associated with an Environment
        for($i=0; $i -lt $environments.Count; $i++)
        {
            $environments[$i] | Add-Member -NotePropertyName Robots -NotePropertyValue (Get-UiPathEnvironmentRobot -EnvironmentId $environments[$i].Id)
        }
        Write-Debug "<$($folder.FullyQualifiedName)> Found environments: $($environments.Count)"

        $folder | Add-Member -NotePropertyName Users -NotePropertyValue $usersStructure
        $folder | Add-Member -NotePropertyName Queues -NotePropertyValue $queues
        $folder | Add-Member -NotePropertyName Assets -NotePropertyValue $assets
        $folder | Add-Member -NotePropertyName Processes -NotePropertyValue $processes
        $folder | Add-Member -NotePropertyName Robots -NotePropertyValue $robots
        $folder | Add-Member -NotePropertyName Triggers -NotePropertyValue $triggers
        $folder | Add-Member -NotePropertyName Environments -NotePropertyValue $environments

    }
    
    $tenantStructure = New-Object PSObject
    $tenantStructure | Add-Member -NotePropertyName Calendars -NotePropertyValue $calendars
    $tenantStructure | Add-Member -NotePropertyName Machines -NotePropertyValue $machines
    $tenantStructure | Add-Member -NotePropertyName Packages -NotePropertyValue $packages
    $tenantStructure | Add-Member -NotePropertyName Libraries -NotePropertyValue $libraries
    $tenantStructure | Add-Member -NotePropertyName Roles -NotePropertyValue $roles
    $tenantStructure | Add-Member -NotePropertyName Users -NotePropertyValue $users
    $tenantStructure | Add-Member -NotePropertyName Folders -NotePropertyValue $folders

    return $tenantStructure
}


function GenerateTenantStructure
{
    Param([PSObject] $tenantConfig, [PSObject] $tenantStructure)

    Write-Host "Generating Tenant Structure for $($tenantConfig.url) $($tenantConfig.tenant)"

    $authResponse = Get-UiPathAuthToken -URL $tenantConfig.url -TenantName $tenantConfig.tenant -Username $tenantConfig.username -Password $tenantConfig.password -Session
    $currentUser = Get-UiPathUser -UserName $tenantConfig.username

    foreach($calendar in $tenantStructure.Calendars)
    {
        try
        {
            # Calendars are not yet supported in the UiPath.Powershell
            $response = Invoke-WebRequest -Uri ($tenantConfig.url + "/odata/Calendars") -Method Get -ContentType "application/json" -Headers @{"Authorization"="Bearer " + $authResponse.Token}
            $calendars = (ConvertFrom-Json $response.Content).value
            if ($calendars.Name -notcontains $calendar.Name)
            {
                #$calendar.PSObject.Properties.Remove("Id")
                $postBody = ConvertTo-Json $calendar
                $response = Invoke-WebRequest -Uri ($tenantConfig.url + "/odata/Calendars") -Method Post -ContentType "application/json" -Body $postBody -Headers @{"Authorization"="Bearer " + $authResponse.Token}                            
                Write-Debug "Added calendar: <$($calendar.Name)>"
            }
            else
            {
                Write-Debug "Skipped calendar: <$($calendar.Name)>"
            }
        }
        catch
        {
            Write-Error "ERROR adding calendar: <$($calendar.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)" 
        }
        
    }

    foreach($machine in $tenantStructure.Machines)
    {
        try
        {
            if ((Get-UiPathMachine -Name $machine.Name) -eq $null)
            {
                Add-UiPathMachine -Name $machine.Name -Type $(If ($machine.Type -eq 1) {"Template"} Else {"Standard"}) -NonProductionSlots $machine.NonProductionSlots -UnattendedSlots $machine.UnattendedSlots 1>$null
                Write-Debug "Added machine: <$($machine.Name)>"
            }
            else
            {
                Write-Debug "Skipped machine: <$($machine.Name)>"
            }
            
        }
        catch
        {
            Write-Error "ERROR adding machine: <$($machine.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)" 
        }
    }

    foreach($role in $tenantStructure.Roles)
    {
        try
        {
            if ((Get-UiPathRole -Name $role.Name) -eq $null)
            {
                Add-UiPathRole -Name $role.Name -Permissions $role.Permissions 1>$null
                Write-Debug "Added role: <$($role.Name)>"
            }
            else
            {
                Write-Debug "Skipped role: <$($role.Name)>"
            }
        }
        catch
        {
            Write-Error "ERROR adding role: <$($role.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)" 
        }
    }
    
    foreach($user in $tenantStructure.Users)
    {
        try
        {
            if ((Get-UiPathUser -Username $user.UserName) -eq $null)
            {
                if($user.UserName -notmatch "\\") #AD users should be imported from the AD
                {
                    Write-Warning "Adding user <$($user.UserName)> with an empty password"
                    Add-UiPathUser -Username $user.UserName -Name $user.Name -Surname $user.Surname -EmailAddress $user.EmailAddress -RolesList $user.RolesList 1>$null    
                }
                else
                {
                    Write-Debug "Skipped AD user: <$($user.UserName)>" 
                }
            }
            else
            {
                Write-Debug "Skipped user: <$($user.UserName)>" 
            }
        }
        catch
        {
            Write-Error "ERROR adding user: <$($user.UserName)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)" 
        }
    }

    foreach($folder in $tenantStructure.Folders)
    {
        $currentFolder = Get-UiPathFolder -DisplayName $folder.DisplayName
        try
        {
            if ($currentFolder -eq $null)
            {
                $currentFolder = Add-UiPathFolder -DisplayName $folder.DisplayName -Description $folder.Description -ProvisionType $folder.ProvisionType -PermissionModel $folder.PermissionModel -ParentId $folder.ParentId
                Write-Debug "Added folder: <$($folder.DisplayName)>"
            }
            else
            {
                Write-Debug "Skipped folder: <$($folder.DisplayName)>"
            }
        }
        catch
        {
            Write-Error "ERROR adding folder: <$($folder.DisplayName)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)" 
        }

        
        $currentFolderUsers = $currentFolder | Get-UiPathFolderUsers
        foreach($user in $folder.Users)
        {
            try
            {
                $currentUser = Get-UiPathUser -UserName $user.UserEntity.UserName
                if ($currentFolderUsers.UserEntity.UserName -notcontains $user.UserEntity.UserName)
                {
                    $roles = @()
                    # Roles are needed only for modern folders
                    if ($folder.ProvisionType -ne "Manual") #Dynamic / Modern Folders
                    {
                        $roles = Get-UiPathRole | Where-Object {$user.Roles.Name -contains $_.Name}
                    }

                    Add-UiPathFolderUserRoles -Folder $currentFolder -UserIds $currentUser.Id -RoleIds $roles

                    #$currentOrgUnit = Get-UiPathOrganizationUnit -DisplayName $currentFolder.DisplayName
                    #Edit-UiPathOrganizationUnitUser -OrganizationUnit $currentOrgUnit -AddUserIds @($currentUser.Id) 1>$null

                    Write-Debug "Added roles for user: <$($currentUser.Id)>"
                }
                else
                {
                    Write-Debug "Skipped adding roles for user: <$($currentUser.Id)>"
                }
            }
            catch
            {
                Write-Error "ERROR adding roles for user: <$($currentUser.Id)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
            }
        }
        
        # Authenticate in the current folder
        $authResponse = Get-UiPathAuthToken -URL $tenantConfig.url -TenantName $tenantConfig.tenant -Username $tenantConfig.username -Password $tenantConfig.password -Session -OrganizationUnit $folder.FullyQualifiedName
        
        foreach($queue in $folder.Queues)
        {
            try
            {
                if ((Get-UiPathQueueDefinition -Name $queue.Name) -eq $null)
                {
                    if ($queue.EnforceUniqueReference)
                    {
                        if ($queue.MaxNumberOfRetries -gt 0)
                        {
                            Add-UiPathQueueDefinition -Name $queue.Name -Description $queue.description -EnforceUniqueReference -AcceptAutomaticallyRetry -MaxNumberOfRetries $queue.MaxNumberOfRetries 1>$null
                        }
                        else
                        {
                            Add-UiPathQueueDefinition -Name $queue.Name -Description $queue.description -EnforceUniqueReference 1>$null
                        }
                    }
                    else
                    {
                        if ($queue.MaxNumberOfRetries -gt 0)
                        {
                            Add-UiPathQueueDefinition -Name $queue.Name -Description $queue.description -AcceptAutomaticallyRetry -MaxNumberOfRetries $queue.MaxNumberOfRetries 1>$null
                        }
                        else
                        {
                            Add-UiPathQueueDefinition -Name $queue.Name -Description $queue.description 1>$null
                        }
                    }
                    Write-Debug "Added queue: <$($queue.Name)>"
                }
                else
                {
                    Write-Debug "Skipped queue: <$($queue.Name)>"
                }
            }
            catch
            {
                Write-Error "ERROR adding queue: <$($queue.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
            }
        }

        foreach($robot in $folder.Robots)
        {
            try
            {
                if ((Get-UiPathRobot -Name $robot.Name) -eq $null)
                {
                    # Workaround to include all machine details (only available when filtering by Id)
                    $existingMachine = Get-UiPathMachine -Id (Get-UiPathMachine -Name $robot.MachineName).Id

                    Write-Warning "Adding Robot <$($robot.Name)> with the default configured  password"
                    Add-UiPathRobot -Name $robot.Name -MachineName $robot.MachineName -Username $robot.Username -Password $tenantConfig.DefaultTenantPassword -Type $robot.Type -HostingType $robot.HostingType -Description $robot.Description -CredentialType "Default" -LicenseKey $existingMachine.LicenseKey 1>$null
                    Write-Debug "Added robot: <$($robot.Name)>"
                }
                else
                {
                    Write-Debug "Skipped robot: <$($robot.Name)>"
                }
            }
            catch
            {
                Write-Error "ERROR adding robot: <$($robot.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
            }
        }

        foreach($environment in $folder.Environments)
        {
            try
            {
                if ((Get-UiPathEnvironment -Name $environment.Name) -eq $null)
                {
                    Add-UiPathEnvironment -Name $environment.Name -Description $environment.Description -Type $environment.Type 1>$null
                    $currentEnvironment = Get-UiPathEnvironment -Name $environment.Name
                    foreach($robot in $environment.Robots)
                    {
                        Add-UiPathEnvironmentRobot -Environment $currentEnvironment -Robot (Get-UiPathRobot -Name $robot.Name) 1>$null
                    }
                    Write-Debug "Added environment : <$($environment.Name)>"
                }
                else
                {
                    Write-Debug "Skipped environment : <$($environment.Name)>"
                }
            }
            catch
            {
                Write-Error "ERROR adding environment: <$($environment.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
            }
        }

        foreach($process in $folder.Processes)
        {
            try
            {
                if ((Get-UiPathProcess -Name $process.ProcessId) -eq $null)
                {
                    $currentEnvironment = Get-UiPathEnvironment -Name ($folder.Environments | Where-Object {$_.Id -eq $process.EnvironmentId}).Name
                    $currentPackage = Get-UiPathPackage -Id $process.ProcessId
                    Add-UiPathProcess -Name $process.ProcessId -Environment $currentEnvironment -PackageVersion $process.ProcessVersion -Package $currentPackage 1> $null
                    Write-Debug "Added process: <$($process.ProcessId)>"
                }
                else
                {
                    Write-Debug "Skipped process: <$($process.ProcessId)>"
                }
            }
            catch
            {
                Write-Error "ERROR adding process: <$($process.ProcessId)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
            }
        }
        
        foreach($trigger in $folder.Triggers)
        {
            try
            {
                if((Get-UiPathProcessSchedule -Name $trigger.Name) -eq $null)
                {
                    # Overwrite the ReleaseId with the one from the current tenant\folder
                    $trigger.ReleaseId = (Get-UiPathProcess -Name ($folder.Processes | Where-Object {$_.Name -eq $trigger.ReleaseName}).ProcessId).Id
                
                    if ($trigger.QueueDefinitionId -ne $null)
                    {
                        # Overwrite the QueueDefinition with the one from the current tenant\folder
                        $trigger.QueueDefinitionId = (Get-UiPathQueueDefinition -Name ($folder.Queues | Where-Object {$_.Id -eq $trigger.QueueDefinitionId}).Name).Id 
                    }

                    if ($trigger.CalendarId -ne $null)
                    {
                        # Overwrite the CalendarId with the one from the current tenant\folder
                        $response = Invoke-WebRequest -Uri ($tenantConfig.url + "/odata/Calendars") -Method Get -ContentType "application/json" -Headers @{"Authorization"="Bearer " + $authResponse.Token}
                        $currentCalendars = (ConvertFrom-Json $response.Content).value
                        $currentCalendar = $currentCalendars | Where-Object {$_.Name -eq ($tenantStructure.Calendars | Where-Object {$_.Id -eq $trigger.CalendarId}).Name}
                        $trigger.CalendarId = $currentCalendar.Id 
                    }

                    $postBody = ($trigger | Select-Object -Property * -ExcludeProperty ReleaseKey,StartProcessNextOccurrence,ExternalJobKey,TimeZoneIana,CalendarName,Id) | ConvertTo-Json
                    Invoke-WebRequest -Uri ($tenantConfig.url + "/odata/ProcessSchedules") -Method Post -ContentType "application/json" -Body $postBody -Headers @{"Authorization"="Bearer " + $authResponse.Token; "X-UIPATH-OrganizationUnitId"=$currentFolder.Id} 1>$null
                    Write-Debug "Added trigger: <$($trigger.Name)>"
                }
                else
                {
                    Write-Debug "Skipped trigger: <$($trigger.Name)>"
                }
            }
            catch
            {
                Write-Error "ERROR adding trigger: <$($trigger.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
            }
        }

        foreach($asset in $folder.Assets)
        {
            try
            {
                if ((Get-UiPathAsset -Name $asset.Name | Where-Object {$_.Name -eq $asset.Name}) -eq $null)
                {
                    if ($asset.RobotValues)
                    {
                        $robotValues = New-Object System.Collections.ArrayList
                        foreach($robotValue in $asset.RobotValues)
                        {
                            $currentRobot = Get-UiPathRobot -Name $robotValue.Name
                        
                            if ($currentRobot -ne $null)
                            {
                                switch ($asset.ValueType)
                                {
                                "Text" { $robotAsset = New-UiPathAssetRobotValue -RobotId $currentRobot.Id -TextValue $robotValue.Value}
                                "Bool" { $robotAsset = New-UiPathAssetRobotValue -RobotId $currentRobot.Id -BoolValue ([boolean] $robotValue.Value)}
                                "Integer" { $robotAsset = New-UiPathAssetRobotValue -RobotId $currentRobot.Id -IntValue $robotValue.Value}
                                "Credential"{
                                        Write-Warning "Adding credential asset <$($asset.Name)> for robot <$($robotValue.Name)> with the default configured password"
                                        $password = ConvertTo-SecureString -String $tenantConfig.DefaultPassword -AsPlainText -Force
                                        $creds = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList ($robotValue.Value -replace "username: ", ""), $password
                                        $robotAsset = New-UiPathAssetRobotValue -RobotId $currentRobot.Id -Credential $creds
                                            }
                                }

                                $robotValues.Add($robotAsset) 1>$null
                            }
                        }
                    
                        if ($robotValues.Count -gt 0)
                        {
                            Add-UiPathAsset -Name $asset.Name -RobotValues $robotValues  1>$null
                        }
                    }
                    else
                    {
                        if ($asset.Value -eq $null)
                        {
                            Write-Warning "Skipping empty value asset: <$($asset.Name)>"
                            continue
                        }

                        switch ($asset.ValueType)
                        {
                        "Text" { Add-UiPathAsset -Name $asset.Name -TextValue $asset.Value > $null}
                        "Bool" { Add-UiPathAsset -Name $asset.Name -BoolValue ([boolean] $asset.Value) > $null}
                        "Integer" { Add-UiPathAsset -Name $asset.Name -IntValue $asset.Value > $null}
                        "Credential" {
                                Write-Warning "Adding credential asset <$($asset.Name)> with the default configured  password"
                                $password = ConvertTo-SecureString -String $tenantConfig.DefaultPassword -AsPlainText -Force
                                $creds = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $asset.Credential.UserName, $password
                                Add-UiPathAsset -Name $asset.Name -Credential $creds 1>$null
                            }
         
                        }
                    }
                    Write-Debug "Added asset: <$($asset.Name)>"
                }
                else
                {
                    Write-Debug "Skipped asset: <$($asset.Name)>"
                }
            }
            catch
            {
                Write-Error "ERROR adding asset: <$($asset.Name)>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
            }
        }
    }
}


function CopyOrchestratorPackages
{
    Param([PSObject] $sourceTenantConfig, [PSObject] $destinationTenantConfig, [PSObject] $sourceTenantStructure, [PSObject] $sourcePackagesFilenames, [PSObject] $sourceLibrariesFilenames)

    Write-Host "Copying Orchestrator packages from $($sourceTenantConfig.url) $($sourceTenantConfig.tenant) to $($destinationTenantConfig.url) $($destinationTenantConfig.tenant)"

    # For a large number of files to be uploaded, re-authentication is required periodically
    $authTime = Get-Date -Year 1970 #seeting this to ensure authentication at first iteration
    
    foreach($package in $sourceTenantStructure.Packages)
    {
        # If we have less than 5 minutes before token expiration, perform an authentication
        if (((Get-Date) - $authTime).TotalMinutes -gt ($destinationTenantConfig.tokenExpirationInMinutes - 5))
        {
            $authTime = Get-Date
            $authenticationResponse = Get-UiPathAuthToken -URL $destinationTenantConfig.url -TenantName $destinationTenantConfig.tenant -Username $destinationTenantConfig.username -Password $destinationTenantConfig.password -Session
        }
        try
        {
            $packageName = ("\\" + $package.Id +"." + $package.Version + ".nupkg")
            # TODO - potential change required
            # If packages have been added manually, it's possible to get multiple matches of the same. This way we are always adding only the first
            $packageFilepath = $sourcePackagesFilenames | Where-Object {$_ -match $packageName | Select -First 1}
            if ((Get-UiPathPackageVersion -Id $package.Id -Version $package.Version) -eq $null)
            {
                Add-UiPathPackage -PackageFile $packageFilepath
                Write-Debug "Added package <$packageName>"
            }
            else
            {
                Write-Debug "Skipped package <$packageName>"
            }
        }
        catch
        {
            Write-Error "ERROR adding package <$packageName>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
        }
    }

    
    foreach($library in $sourceTenantStructure.Libraries)
    {   
        # If we have less than 5 minutes before token expiration, perform another authentication
        if (((Get-Date) - $authTime).TotalMinutes -gt ($destinationTenantConfig.tokenExpirationInMinutes - 5))
        {
            $authTime = Get-Date
            $authenticationResponse = Get-UiPathAuthToken -URL $destinationTenantConfig.url -TenantName $destinationTenantConfig.tenant -Username $destinationTenantConfig.username -Password $destinationTenantConfig.password -Session
        }
        try
        {
            $libraryName = ("\\" + $library.Id +"." + $library.Version + ".nupkg")
            # TODO - potential change required
            # If libraries have been added manually, it's possible to get multiple matches of the same. This way we are always adding only the first
            $libraryFilepath = $sourceLibrariesFilenames | Where-Object {$_ -match $libraryName | Select -First 1}
            $response = Invoke-WebRequest -Uri ($destinationTenantConfig.url + "/odata/Libraries/UiPath.Server.Configuration.OData.GetVersions(packageId='$($library.Id)')") -Method Get -ContentType "application/json" -Headers @{"Authorization"="Bearer " + $authenticationResponse.Token}
            if (((ConvertFrom-Json $response.Content).value | Where-Object {$_.Version -eq $library.Version}) -eq $null)
            {
                Add-UiPathLibrary -LibraryPackage $libraryFilepath
                Write-Debug "Added library <$libraryName>"
            }
            else
            {
                Write-Debug "Skipped library <$libraryName>"
            }
        }
        catch
        {
            Write-Error "ERROR adding library <$libraryName>: $_ at line $($_.InvocationInfo.ScriptLineNumber)"
        }
    }
}

####################### Execution Steps #######################

# Generate the Tenant Structure from the Source Tenant
$sourceTenantStructure = GetTenantStructure $SourceTenantConfig 

# OPTIONAL # Save the Structure to a Json file
#$sourceTenantStructure | Convertto-json -Depth 10 | Out-File $PSScriptRoot\"sourceTenant.json"

# OPTIONAL # Read the Structure From the Json file
#$sourceTenantStructure = Get-Content -Path $PSScriptRoot\"sourceTenant.json" | ConvertFrom-Json

$sourcePackagesFilenames = (Get-ChildItem -Path $sourceTenantConfig.packagesPath -Force -Recurse -Filter "*.nupkg").FullName

# OPTIONAL # Save & Read the Package filenames from a folder. For thousands of packages this procedure can take a long time, so in this way it speeds up debugging
#$sourcePackagesFilenames | Convertto-json -Depth 10 | Out-File $PSScriptRoot\"sourceTenantPackages.json"
#$sourcePackagesFilenames = Get-Content -Path $PSScriptRoot\"sourceTenantPackages.json" | ConvertFrom-Json

$sourceLibrariesFilenames = (Get-ChildItem -Path $sourceTenantConfig.librariesPath -Force -Recurse -Filter "*.nupkg").FullName

# OPTIONAL # Save & Read the Library filenames from a folder. For thousands of packages this procedure can take a long time, so in this way it speeds up debugging
#$sourceLibrariesFilenames | Convertto-json -Depth 10 | Out-File $PSScriptRoot\"sourceTenantLibraries.json"
#$sourceLibrariesFilenames = Get-Content -Path $PSScriptRoot\"sourceTenantLibraries.json" | ConvertFrom-Json


###########
# WARNING # 
# WARNING # 
# WARNING # The following section will makes changes to the destination tenant structure
# WARNING # 
# WARNING # 
###########

# Copy Packages from Source Tenant to Destination Tenant
#CopyOrchestratorPackages $SourceTenantConfig $DestinationTenantConfig $sourceTenantStructure $sourcePackagesFilenames $sourceLibrariesFilenames
# Re-Generate the structure on the Detination Tenant
#GenerateTenantStructure $DestinationTenantConfig $sourceTenantStructure

###############################################################