<#
    .SYNOPSIS
        Reads the CSV file containing tags and uploads a dummy asset with the respective tags.
    .DESCRIPTION
        You should first import the UiPath.PowerShell module and authenticate yourself with your Orchestrator using Get-UiPathAuthToken before running this script.
        This is a an example script provided as-is.
    .PARAMETER TagsFilePath
        The CSV file containing the tags
    .EXAMPLE
        .\Add-DummyAsset-WithTags.ps1 -TagsFilePath '<pathToFile>'
#>

param(
        [Parameter(Mandatory=$true)] 
        [string] $TagsFilePath
)

$AssetNameGuid = New-Guid

$Asset = New-Object -TypeName psobject
$Asset | Add-Member -MemberType NoteProperty -Name StringValue -Value 'DummyValue'
$Asset | Add-Member -MemberType NoteProperty -Name ValueType -Value 'Text'
$Asset | Add-Member -MemberType NoteProperty -Name Name -Value "Dummy Asset $($AssetNameGuid)"
$Asset | Add-Member -MemberType NoteProperty -Name ValueScope -Value 'Global'
$Asset | Add-Member -MemberType NoteProperty -Name HasDefaultValue -Value $true
$Asset | Add-Member -MemberType NoteProperty -Name Tags -Value @()

Import-Csv -Path $TagsFilePath | ForEach-Object {
    Write-Verbose "Tag with Name: $($_.Name) has Value: $($_.Value)"
    $Tag = New-Object -TypeName psobject
    $Tag | Add-Member -MemberType NoteProperty -Name Name -Value $_.Name
    $Tag | Add-Member -MemberType NoteProperty -Name DisplayName -Value $_.Name
    $Tag | Add-Member -MemberType NoteProperty -Name Value -Value $_.Value
    $Tag | Add-Member -MemberType NoteProperty -Name DisplayValue -Value $_.Value
    $Asset.Tags += $Tag
}

$AssetJSON = ConvertTo-Json -InputObject $Asset

$token = Get-UiPathAuthToken -CurrentSession

$assetsUrl = "$($token.URL)/odata/Assets"

if ($token.WindowsCredentials) {
    Write-Verbose "Invoke-WebRequest $assetsUrl -UseDefaultCredentials"
    $response = Invoke-WebRequest $assetsUrl -UseDefaultCredentials -Method POST -Body $AssetJSON -ContentType "application/json"
} else {
    Write-Verbose "Invoke-WebRequest $assetsUrl  -Headers @{Authorization = 'Bearer ...'"
    $response = Invoke-WebRequest $assetsUrl -Headers @{Authorization = "Bearer $($token.Token)"} -Method POST -Body $AssetJSON -ContentType "application/json"
}