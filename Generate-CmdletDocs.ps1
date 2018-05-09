
$cmdlets = Get-Command -Module UiPath.PowerShell | select Name

$file = "docs\Home.md"
"" | Out-File $file -Encoding utf8
foreach($cmdlet in $cmdlets)
{
	"- [$($cmdlet.Name)]($($cmdlet.Name).md)" | Out-File  $file -Encoding utf8 -Append
}

foreach($cmdlet in $cmdlets) 
{
	$file = "docs\$($cmdlet.Name).md"
	"``````PowerShell" | Out-File $file -Encoding utf8
	Get-Help $cmdlet.Name -full | Out-File $file -Encoding utf8 -Append
	"``````" | Out-File $file -Encoding utf8 -Append
}