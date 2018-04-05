# UiPath Orchestrator PowerShell library

A PowerShell library for interacting with [UiPath Orchestrator](https://orchestrator.uipath.com/).

# Getting Started

Download the desired version module from the [Releases](https://github.com/UiPath/orchestrator-powershell/releases) page, or build solution to obtain the `UiPath.PowerShell.dll` module. Import the module in PowerShell:

```PowerShell
PS C:\>Import-Module UiPath.PowerShell.dll
```

Use the PowerShell `Get-Command` to obtain all cmdlets exported by the module:

```PowerShell
PS C:\>Get-Help -Module UiPath.PowerShell
```

You can obtain each command syntax using PowerShell's own `Get-Help`:

```PowerShell
PS C:\>Get-Help Add-UiPathRobot
```

To start using the library, you need to connect first to a running Orchestrator instance. Use the [`Get-UiPathAuthToken`](docs/Get-UiPathAuthToken.md) cmdlet:
```PowerShell
PS C:\>Get-UiPathAuthToken -URL <orchestratorurl> -Username <OrchestratorUser> -Password <password> -Session
```

The `-Session` flag makes the authentication persist on the PowerShell session for up to 30 minutes. After this you will not have to authenticate again each cmdlet. Some examples:

```PowerShell
S C:\> Get-UiPathRobot | Format-Table

 Id LicenseKey MachineName    Name           Description
 -- ---------- -----------    ----           -----------

132            RERUSANU       PwdRobot1
133            RERUSANU       PwdRobot2
134            RERUSANU       PwdRobot3
```

For more example, see the [docs](docs/Home.md)

# Prerequisites

To build the project, In addition to the C# SDK (the solution is Visual Studio 2017 based) you will need [autorest](https://github.com/Azure/autorest).
To use the library you won't need anything but the build artifacts.

# License

This project is copyright [UiPath INC](https://uipath.com) and licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
