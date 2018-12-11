# UiPath Orchestrator PowerShell library

A PowerShell library for interacting with [UiPath Orchestrator](https://orchestrator.uipath.com/).

```PowerShell
PS C:\>Install-PackageProvider -Name NuGet -Force
PS C:\>Register-PSRepository -Name UiPath -SourceLocation https://www.myget.org/F/uipath-dev/api/v2
PS C:\>Install-Module -Repository UiPath -Name UiPath.Powershell -Force
PS C:\>Import-Module UiPath.PowerShell
PS C:\>Get-UiPathAuthToken -URL <orchestratorurl> -Username <OrchestratorUser> -Password <password> -Session
PS C:\>Get-UiPathRobot
```

# Using the UiPath.PowerShell module

The full documentation is in [docs](docs/Home.md)

Use the PowerShell `Get-Command` to obtain all cmdlets exported by the module:

```PowerShell
PS C:\>Get-Command -Module UiPath.PowerShell
```

You can obtain each command syntax using PowerShell's own `Get-Help`:

```PowerShell
PS C:\>Get-Help Add-UiPathRobot
```

To start using the library, you need to connect first to a running Orchestrator instance. Use the [`Get-UiPathAuthToken`](docs/Get-UiPathAuthToken.md) cmdlet:
```PowerShell
PS C:\>Get-UiPathAuthToken -URL <orchestratorurl> -Username <OrchestratorUser> -Password <password> -Session
```

To connect to an Orchestrator instance using integrated AD and SSO, use the `-WindowsCredentials` argument to obtain the token. The library will authenticate to Orchestrator as the Windows user running the PowerShell session:
```PowerShell
PS C:\>Get-UiPathAuthToken -URL <orchestratorurl> -WindowsCredentials -Session
```

The `-Session` flag makes the authentication persist on the PowerShell session for up to 30 minutes. After this you will not have to authenticate again each cmdlet. Some examples:

Use `Get-UiPathAuthToken ... -TenantName <tenantName>` for multi-tenant Orchetsrator deployments.
Use `Get-UiPathAuthToken ... -OrganizationUnit <OUName>` for Orchetsrator deployments with OrganizationUnits enabled.

```PowerShell
PS C:\> Get-UiPathRobot | Format-Table

 Id LicenseKey MachineName    Name           Description
 -- ---------- -----------    ----           -----------

132            RERUSANU       PwdRobot1
133            RERUSANU       PwdRobot2
134            RERUSANU       PwdRobot3
```

For more example, see the [docs](docs/Home.md)

# Getting Started

## Automated install

### Register the UiPath Gallery as a NuGet module provider

The UiPath.PowerShell module can be installed as a NuGet package. You will need to run once these commands first:

```PowerShell
PS C:\>Install-PackageProvider -Name NuGet -Force -Scope CurrentUser
PS C:\>Register-PSRepository -Name UiPath -SourceLocation https://www.myget.org/F/uipath-dev/api/v2
```

Note that in the example above the use of `-Scope CurrentUser` means that the NuGet package provider is registered only for the current user. This does not require administrative privilegs, but the registration is transient. You may opt instead to run from an elevated prompt and remove the `-Scope CurrentUser` for a permanent registration.

You can validate that the NuGet package provider and the UiPath Gallery repository are registered:

```PowerShell
PS C:\>Get-PackageProvider -Name NuGet

Name                     Version          DynamicOptions
----                     -------          --------------
NuGet                    2.8.5.208        Destination, ExcludeVersion, Scope, SkipDependencies, Headers, FilterOnTag...

PS C:>Get-PSRepository

Name                      InstallationPolicy   SourceLocation
----                      ------------------   --------------
PSGallery                 Untrusted            https://www.powershellgallery.com/api/v2
UiPath                    Untrusted            https://www.myget.org/F/uipath-dev/api/v2
````

### Install the UiPath.PowerShell module using the UiPath repository

```PowerShell
PS C:\>Install-Module -Repository UiPath -Name UiPath.Powershell -Force  -Scope CurrentUser
PS C:\>Import-Module UiPath.PowerShell
```

This command will download, install and import the UiPath.PowerShell module. Again, you can opt to use a global scope by removing the `-Scope CurrentUser` argument, but this will require running the command an elevated prompt.

You can validate that the module was downloaded, installed and loaded:
```PowerShell
PS c:\>Get-Module UiPath.PowerShell

ModuleType Version    Name                                ExportedCommands
---------- -------    ----                                ----------------
Binary     18.3.2.... UiPath.PowerShell                   {Add-UiPathAsset, Add-UiPathEnvironment, Add-UiPathEnviron...

```

# License

This project is copyright [UiPath INC](https://uipath.com) and licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

# Prerequisites for building from source

To build the project, In addition to the C# SDK (the solution is Visual Studio 2017 based) you will need [autorest](https://github.com/Azure/autorest).
To use the library you won't need anything but the build artifacts.
