```PowerShell

NAME
    Get-UiPathMaintenance
    
SYNOPSIS
    Maintenance session summary log.
    
    
SYNTAX
    Get-UiPathMaintenance [-AuthToken <AuthToken>] [-RequestTimeout <int>] [<CommonParameters>]
    
    
DESCRIPTION
    This cmdlet will return a summary of the current or last Maintenance session with the following structure
    
    State: None | Draining | Suspended
    
    MaintenanceLogs : {{ State = None, TimeStamp = }, { State = Draining, TimeStamp = }, { State = Suspended, TimeStamp = }}
    
    JobsStopped : 0
    
    JobsKilled : 0
    
    SchedulesFired : 0
    
    SystemSchedulesFired : 0
    

PARAMETERS
    -AuthToken <AuthToken>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -RequestTimeout <int>
        
        Required?                    false
        Position?                    named
        Default value                100
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
INPUTS
    
OUTPUTS
    
    
RELATED LINKS



```
