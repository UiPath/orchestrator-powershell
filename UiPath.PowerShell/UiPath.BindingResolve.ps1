$OnAssemblyResolve = [System.ResolveEventHandler] {
  param($sender, $e)


  if ($e.Name.StartsWith("System.Net.Http,"))
  {
    [system.diagnostics.debug]::WriteLine("Binding redirect $($e.Name) resolved to $($script:httpNet.Location)")
    return $script:httpNet
  }

  return $null
}

trap [System.Exception] { [system.diagnostics.debug]::WriteLine("Exception: $_") }

if ($env:UIPATH_POWERSHELL_SKIP_BINDING_REDIRECT -ne $true)
{
  Write-Verbose "Installing assembly resolve binding redirect callback"
  # See https://github.com/dotnet/runtime/issues/20777#issuecomment-338418610 
  $script:httpNet = [reflection.assembly]::Load("System.Net.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
  [System.AppDomain]::CurrentDomain.add_AssemblyResolve($OnAssemblyResolve)
}
