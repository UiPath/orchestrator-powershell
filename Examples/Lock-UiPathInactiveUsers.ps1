<#
    .SYNOPSIS
        Mark as inactive users in Orchestrator with no activity in last X days
    .DESCRIPTION
        You must run this script as an user that has the privilege to edit users. This privilege can be delegated to you by a domain administrator.
        You should first import the UiPath.PowerShell module and authenticate yourself with your Orchestrator using Get-UiPathAuthToken before running this script.
        Users with Administrator roles are skipped
    .PARAMETER Days
        Number of days of inactivity. Default is 90 days
#>
param(
    [Parameter()] [int]$Days = 90
)

$ErrorActionPreference = "Stop"

try {
    $datetime = (Get-Date).ToUniversalTime().AddDays(-$Days)
    Write-Verbose "Last activity acceptead date is $datetime"

    # Discover all active users
    Write-Verbose "Get-UiPathUser -Type User -IsActive $true | where {$_.Username -ne 'Admin'}"

    $allUsers = Get-UiPathUser -Type User -IsActive $true | where {$_.Username -ne "Admin"}

    foreach ($user in $allUsers) {
        if ($user.RolesList -contains 'Administrator') {
            Write-Verbose "Skipping user $($user.Name). Reason: Administrator"
        }
        else {
            $lastActivityDate = $user.LastLoginTime
            if ($null -eq $lastActivityDate) {
                $lastActivityDate = $user.CreationTime
            }

            if ($datetime -gt $lastActivityDate) {

                Write-Verbose "Lock-UiPathUser -Id $($user.Id)"
                Lock-UiPathUser -Id $user.Id
            }`
            else {
                Write-Verbose "Skipping user $($user.Name). Reason: Active in the last $Days Days"
            }
        }
    }
}
catch {
    $e = $_.Exception
    $klass = $e.GetType().Name
    $line = $_.InvocationInfo.ScriptLineNumber
    $script = $_.InvocationInfo.ScriptName
    $msg = $e.Message

    Write-Error "$klass $msg ($script $line)"
}
