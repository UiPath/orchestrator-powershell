
<#
    .SYNOPSIS
        Updates AD passwords for UiPath Orchestrator robot credentials that are close to password expiration
    .DESCRIPTION
        You must run this script as an user that has the privilege to reset the robot passwords in the AD. This privilege can be delegated to you by a domain administrator.
        You should first import the UiPath.PowerShell module and authenticate yourself with your Orchestrator using Get-UiPathAuthToken before running this script.
    .PARAMETER DomainName
        The domain to sync users with. It does not necessarily has to be your current user or machine domain, but there must be some trust relationship so your Windows session can discover and interogate this domain AD.
    .PARAMETER Days
        Update the passwords in AD for the robot credentials that expire sooner than this number of days. Default is 1 Day.
    .PARAMETER PasswordLength
        The total number of characters for the generated password(s). Default is 16 characters.
    .PARAMETER NonAlphaCount
        Number of non-alphanumeric characters in the generated password(s). Default is 3 characters.
    .EXAMPLE
        Update-UiPathADRobotPasswords MyDomain
        Updates the passwords for all robots that use credentials from the domain MyDomain
    .EXAMPLE
        Update-UiPathADRobotPasswords MyDomain -Days 7
        Updates the passwords for all robots that use credentials that will expire sooner than 7 days
#>
param(
    [Parameter(Mandatory=$true, Position=0)] [string] $DomainName,
    [Parameter()] [int]$Days = 1,
    [Parameter()] [int]$PasswordLength = 16,
    [Parameter()] [int]$NonAlphaCount = 3
)

$ErrorActionPreference = "Stop"

try
{
    Write-Verbose "Get-ADDomain -Identity $domainName"
    $dc = Get-ADDomain -Identity $domainName

    Write-Progress -Activity "Update robot expiring credential AD passwords" `
            -CurrentOperation "Discover robots with credentials in $DomainName" `
            -PercentComplete 33


    # Discover all robots with robot credentials from the requested domain, extract the distinct username used
    Write-Verbose "Get-UiPathRobot | Where-Object {`$_.Username -like '$DomainName\*'} | Select-Object -Property Username -Unique"
    $robotUserNames = Get-UiPathRobot | Where-Object {$_.Username -like "$DomainName\*"} | Select-Object -Property Username -Unique

    Write-Progress -Activity "Update robot expiring credential AD passwords" `
            -CurrentOperation "Analyze robot AD users" `
            -PercentComplete 66


    $i = 1
    foreach($robotUserName in $robotUserNames)
    {
        Write-Progress -Id 1 `
            -Activity "Analyze robot AD users" `
            -CurrentOperation $robotUserName `
            -PercentComplete ($i/$robotUserNames.Count*100)
        $i += 1

        # Discover the password expiration date for the AD user

        $userNameParts = $robotUserName.Username.Split('\')
        $userName = $userNameParts[1]

        # see https://msdn.microsoft.com/en-us/library/cc223410.aspx for msDS-UserPasswordExpiryTimeComputed attribute spec

        Write-Verbose "Get-ADUser -Server $($dc.PDCEmulator) -Identity $userName -Properties `"SamAccountName`",`"msDS-UserPasswordExpiryTimeComputed`""
        $userObject = Get-ADUser -Server $dc.PDCEmulator -Identity $userName -Properties "SamAccountName","msDS-UserPasswordExpiryTimeComputed"

        $expiration = [datetime]::FromFileTime($userObject."msDS-UserPasswordExpiryTimeComputed")

        # skip users not expiring within the given time frame
        if (($userObject."msDS-UserPasswordExpiryTimeComputed" -eq 0) -or ($userObject."msDS-UserPasswordExpiryTimeComputed" -eq 0x7FFFFFFFFFFFFFFF))
        {
            Write-Verbose "Skip:$userName no expiration: $($userObject."msDS-UserPasswordExpiryTimeComputed")"
            continue
        }

        $daysTillExpire = ($expiration - [datetime]::UtcNow).TotalDays
        if ( $daysTillExpire -gt $Days)
        {
            Write-Verbose "Skip:$userName expiration:$expiration days till expire:$daysTillExpire"
            continue
        }

        # generate a random password
        # NB: this generation method does not guarantee to meet the AD required complexity rules
        # Feel free to fix it to meet whatever rules you have in place
        $password =  [System.Web.Security.Membership]::GeneratePassword($PasswordLength, $NonAlphaCount)
        $securePassword = ConvertTo-SecureString -AsPlainText $password -Force

        Write-Verbose "Set-ADAccountPassword -Server $($dc.PDCEmulator) -Identity $userObject -Reset -NewPassword ****"
        Set-ADAccountPassword -Server $dc.PDCEmulator -Identity $userObject -Reset -NewPassword $securePassword

        # discover all robots that use this robot credential
        Write-Verbose "Get-UiPathRobot -Username $($robotUserName.Username)"
        $robots = Get-UiPathRobot -Username $robotUserName.Username
        $j = 1
        foreach($robot in $robots)
        {
            Write-Progress -Id 2 `
                -Activity "Update Orchestrator robots" `
                -CurrentOperation $robot.Name `
                -PercentComplete ($j/$robots.Count*100)
            $j += 1

            #update the robot credential with the new password
            Write-Verbose "Edit-UiPathRobot -Id $($robot.Id) -Password ****"
            Edit-UiPathRobot -Id $robot.Id -Password $password
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