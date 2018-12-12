<#
    .SYNOPSIS
        Extracts the declared dependencies for an Orchestrator package
    .DESCRIPTION
        You should first import the UiPath.PowerShell module and authenticate yourself with your Orchestrator using Get-UiPathAuthToken before running this script.
        This is a an example script provided as-is.
    .PARAMETER Package
        The Orchestrator package. Use Get-UiPathPackage to retrive this object.
    .EXAMPLE
        $package = Get-UiPathPackage -Id <my package name>
        .\Get-UiPathPackageDependencies.ps1 $package
        Lists the package dependencies of a specific package
    .EXAMPLE
        Get-UiPathPackage | ForEach-Object { .\Examples\Get-UiPathPackageDependencies.ps1 $_ | Add-Member Parent $_.Id -PassThru}
        Lists the package dependencies for all packages in Orchestrator
#>

    param(
        [Parameter(Mandatory=$true, Position=0, ValueFromPipeline)] 
        [object] $Package
    )

$ErrorActionPreference = "Stop"


function Get-UiPathDeploymentUrl {
    param(
        [object] $token
    )

    $deploymentUrlSetting = Get-UiPathSetting | where {$_.Name -eq 'DeploymentUrl'}

    if ([string]::IsNullOrWhiteSpace($deploymentUrlSetting.Value))
    {
        $url = "$($token.URL)/nuget/feed/$($token.TenantName.ToLowerInvariant())"

    }
    else
    {
        $url = $deploymentUrlSetting.Value
    }
    Write-Verbose "NuGet Feed: $url"
    return $url
}

    $token = Get-UiPathAuthToken -CurrentSession


    $deploymentUrl = Get-UiPathDeploymentUrl $token

    $specUrl = "$deploymentUrl/Packages(Id='$($Package.Id.ToLowerInvariant())',Version='$($Package.Version.ToLowerInvariant())')"

    if ($token.WindowsCredentials) {
        Write-Verbose "Invoke-WebRequest $specUrl -UseDefaultCredentials"
        $response = Invoke-WebRequest $specUrl -UseDefaultCredentials
    } else {
        Write-Verbose "Invoke-WebRequest $specUrl  -Headers @{Authorization = 'Bearer ...'"
        $response = Invoke-WebRequest $specUrl -Headers @{Authorization = "Bearer $($token.Token)"}
    }
    

    $content = [XML] $response.Content

    $dependencies = $content.entry.properties.Dependencies


    if (-not [string]::IsNullOrWhiteSpace($dependencies))
    {
        $splits = $dependencies.split("|")
        foreach($split in $splits)
        {
            $parts = $split.split(":");
            $lib = New-Object -TypeName psobject
            $lib | Add-Member -MemberType NoteProperty -Name Id $parts[0]
            $lib | Add-Member -MemberType NoteProperty -Name Version $parts[1]
            Write-Output $lib
        }
    }
