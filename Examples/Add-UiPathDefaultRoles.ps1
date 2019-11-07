param (
    [Parameter(Mandatory = $false, Position = 1)]
    [string] $URL,

    [Parameter(Mandatory = $false, Position = 2)]
    [string] $TenantName,

    [Parameter(Mandatory = $false, Position = 3)]
    [string] $Username,

    [Parameter(Mandatory = $false, Position = 4)]
    [string] $Password
)

$RolesPermissions = @{ 
    Execute = "Robots.View",
                "Processes.View",
                "Assets.View",
                "Queues.View",
                "Transactions.View", "Transactions.Create", "Transactions.Edit",
                "Logs.Create",
                "ExecutionMedia.Create",
                "Tasks.View", "Tasks.Create";
    EnableProcessExecute = "License.View",
                            "Settings.View",
                            "Machines.View",
                            "Packages.View", "Packages.Create",
                            "Libraries.View", "Libraries.Create",
                            "Webhooks.View";
    FolderManagement = "Robots.View", "Robots.Create", "Robots.Edit", "Robots.Delete",
                        "Processes.View", "Processes.Create", "Processes.Edit", "Processes.Delete",
                        "Assets.View", "Assets.Create", "Assets.Edit", "Assets.Delete",
                        "Environments.View", "Environments.Create", "Environments.Edit", "Environments.Delete",
                        "Queues.View", "Queues.Create", "Queues.Edit", "Queues.Delete",
                        "Transactions.View", "Transactions.Create", "Transactions.Edit", "Transactions.Delete",
                        "Jobs.View", "Jobs.Create", "Jobs.Edit", "Jobs.Delete",
                        "Schedules.View", "Schedules.Create", "Schedules.Edit", "Schedules.Delete",
                        "ExecutionMedia.View", "ExecutionMedia.Create", "ExecutionMedia.Edit", "ExecutionMedia.Delete",
                        "Logs.View", "Logs.Create", "Logs.Edit", "Logs.Delete",
                        "Monitoring.View", "Monitoring.Create", "Monitoring.Edit", "Monitoring.Delete",
                        "Tasks.View", "Tasks.Create", "Tasks.Edit", "Tasks.Delete",
                        "TaskCatalogs.View", "TaskCatalogs.Create", "TaskCatalogs.Edit", "TaskCatalogs.Delete";
    EnableFolderManagement = "Users.View",
                                "Machines.View",
                                "Roles.View",
                                "Packages.View";
    TenantAdministrator = "License.View", "License.Create", "License.Edit", "License.Delete",
                            "Settings.View", "Settings.Create", "Settings.Edit", "Settings.Delete",
                            "Machines.View", "Machines.Create", "Machines.Edit", "Machines.Delete",
                            "Packages.View", "Packages.Create", "Packages.Edit", "Packages.Delete",
                            "Libraries.View", "Libraries.Create", "Libraries.Edit", "Libraries.Delete",
                            "Roles.View", "Roles.Create", "Roles.Edit", "Roles.Delete",
                            "Users.View", "Users.Create", "Users.Edit", "Users.Delete",
                            "Audit.View", "Audit.Create", "Audit.Edit", "Audit.Delete",
                            "Alerts.View", "Alerts.Create", "Alerts.Edit", "Alerts.Delete",
                            "Units.View", "Units.Create", "Units.Edit", "Units.Delete",
                            "Webhooks.View", "Webhooks.Create", "Webhooks.Edit", "Webhooks.Delete";
    ProcessDesigner = "Libraries", "Libraries.View", "Libraries.Edit", "Libraries.Create",
                        "Machines", "Machines.View", "Machines.Edit", "Machines.Create", "Machines.Delete",
                        "License", "License.View",
                        "Settings", "Settings.View",
                        "Robots", "Robots.View", "Robots.Edit", "Robots.Create", "Robots.Delete",
                        "Processes", "Processes.View", "Processes.Edit", "Processes.Create", "Processes.Delete",
                        "Packages", "Packages.View", "Packages.Edit", "Packages.Create", "Packages.Delete",
                        "Assets", "Assets.View", "Assets.Edit", "Assets.Create", "Assets.Delete",
                        "Environments", "Environments.View", "Environments.Edit", "Environments.Create", "Environments.Delete",
                        "Queues", "Queues.View", "Queues.Edit", "Queues.Create",
                        "Transactions", "Transactions.View", "Transactions.Edit", "Transactions.Create", "Transactions.Delete",
                        "Jobs", "Jobs.View", "Jobs.Edit", "Jobs.Create", "Jobs.Delete",
                        "Schedules", "Schedules.View", "Schedules.Edit", "Schedules.Create",
                        "Logs", "Logs.View",
                        "Roles",
                        "Users",
                        "Audit",
                        "Alerts", "Alerts.View",
                        "Units", "Units.View",
                        "Webhooks", "Webhooks.View", "Webhooks.Edit", "Webhooks.Create", "Webhooks.Delete",
                        "Monitoring", "Monitoring.View",
                        "ExecutionMedia";
}

if (![string]::IsNullOrEmpty($URL)) {
    Get-UiPathAuthToken -URL $URL -TenantName $TenantName -Username $Username -Password $Password -Session
}
                            
Add-UiPathRole -Name 'Execute' -Permissions $RolesPermissions.Execute
Add-UiPathRole -Name 'Enable Process Execute' -Permissions $RolesPermissions.EnableProcessExecute
Add-UiPathRole -Name 'Folder Management' -Permissions $RolesPermissions.FolderManagement
Add-UiPathRole -Name 'Enable Folder Management' -Permissions $RolesPermissions.EnableFolderManagement
Add-UiPathRole -Name 'Tenant Administrator' -Permissions $RolesPermissions.TenantAdministrator
Add-UiPathRole -Name 'ProcessDesigner' -Permissions $RolesPermissions.ProcessDesigner -IsEditable -IsStatic
