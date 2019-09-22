$RolesPermissions = @{ 
    Execute = "Robots.View, 
                Processes.View, 
                Assets.View, 
                Queues.View, 
                Transactions.View, Transactions.Create, Transactions.Edit, 
                Logs.Create, 
                ExecutionMedia.Create, 
                Tasks.View, Tasks.Create"; 
    EnableProcessExecute = "License.View, 
                            Settings.View, 
                            Machines.View, 
                            Packages.View, Packages.Create, 
                            Libraries.View, Libraries.Create, 
                            Webhooks.View";
    FolderManagement = "Robots.View, Robots.Create, Robots.Edit, Robots.Delete, 
                        Processes.View, Processes.Create, Processes.Edit, Processes.Delete, 
                        Assets.View, Assets.Create, Assets.Edit, Assets.Delete, 
                        Environments.View, Environments.Create, Environments.Edit, Environments.Delete, 
                        Queues.View, Queues.Create, Queues.Edit, Queues.Delete, 
                        Transactions.View, Transactions.Create, Transactions.Edit, Transactions.Delete, 
                        Jobs.View, Jobs.Create, Jobs.Edit, Jobs.Delete, 
                        Schedules.View, Schedules.Create, Schedules.Edit, Schedules.Delete, 
                        ExecutionMedia.View, ExecutionMedia.Create, ExecutionMedia.Edit, ExecutionMedia.Delete, 
                        Logs.View, Logs.Create, Logs.Edit, Logs.Delete, 
                        Monitoring.View, Monitoring.Create, Monitoring.Edit, Monitoring.Delete, 
                        Tasks.View, Tasks.Create, Tasks.Edit, Tasks.Delete, 
                        TaskCatalogs.View, TaskCatalogs.Create, TaskCatalogs.Edit, TaskCatalogs.Delete";
    EnableFolderManagement = "Users.View, 
                                Machines.View, 
                                Roles.View, 
                                Packages.View";
    TenantAdministrator = "License.View, License.Create, License.Edit, License.Delete, 
                            Settings.View, Settings.Create, Settings.Edit, Settings.Delete, 
                            Machines.View, Machines.Create, Machines.Edit, Machines.Delete, 
                            Packages.View, Packages.Create, Packages.Edit, Packages.Delete, 
                            Libraries.View, Libraries.Create, Libraries.Edit, Libraries.Delete, 
                            Roles.View, Roles.Create, Roles.Edit, Roles.Delete, 
                            Users.View, Users.Create, Users.Edit, Users.Delete, 
                            Audit.View, Audit.Create, Audit.Edit, Audit.Delete, 
                            Alerts.View, Alerts.Create, Alerts.Edit, Alerts.Delete, 
                            Units.View, Units.Create, Units.Edit, Units.Delete, 
                            Webhooks.View, Webhooks.Create, Webhooks.Edit, Webhooks.Delete" 
}
                            
Add-UiPathRole -Name 'Execute' -Permissions $RolesPermissions.Execute
Add-UiPathRole -Name 'Enable Process Execute' -Permissions $RolesPermissions.EnableProcessExecute
Add-UiPathRole -Name 'Folder Management' -Permissions $RolesPermissions.FolderManagement
Add-UiPathRole -Name 'Enable Folder Management' -Permissions $RolesPermissions.EnableFolderManagement
Add-UiPathRole -Name 'Tenant Administrator' -Permissions $RolesPermissions.TenantAdministrator