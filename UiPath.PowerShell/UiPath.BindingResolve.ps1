$OnAssemblyResolve = [System.ResolveEventHandler] {
  param($sender, $e)

  if ($e.Name.StartsWith("System.Net.Http,"))
  {
    [system.diagnostics.debug]::WriteLine("Binding redirect $($e.Name) resolved to $($script:httpNet.Location)")
    return $script:httpNet
  }

  return $null
}

# See https://github.com/dotnet/runtime/issues/20777#issuecomment-338418610 

if ($env:UIPATH_POWERSHELL_SKIP_BINDING_REDIRECT -ne $true)
{
  $dllPath = "$PSScriptRoot\lib\netstandard2.0\UiPath.PowerShell.dll"
  if (Test-Path $dllPath)
  {
	  Write-Verbose "Installing assembly resolve binding redirect callback, loading: $dllPath"

	  $script:httpNet = [reflection.assembly]::Load("System.Net.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
	  
	  [System.AppDomain]::CurrentDomain.add_AssemblyResolve($OnAssemblyResolve)
	  try
	  {
		  $module = [reflection.assembly]::LoadFrom($dllPath)
		  $null = $module.GetTypes()
	  }
	  finally
	  {
		  [System.AppDomain]::CurrentDomain.remove_AssemblyResolve($OnAssemblyResolve)
		  Write-Verbose "Removed assembly resolve binding redirect callback"
	  }
  }
}
