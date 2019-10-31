```PowerShell

NAME
    Start-UiPathMaintenance
    
SYNOPSIS
    Starts a Maintenance session
    
    
SYNTAX
    Start-UiPathMaintenance [-Phase] <string> [-AuthToken <AuthToken>] [-Force <SwitchParameter>] [-KillJobs <SwitchParameter>] [-RequestTimeout <int>] [<CommonParameters>]
    
    
DESCRIPTION
    This cmdlet will start a Maintenance session for UiPath Orchestrator service.
    

PARAMETERS
    -Phase <string>
        Indicates the Maintenance Mode phase. Draining = Most user and robots API calls will continue to work. A set of API calls whichi would generate additional Robos workloads will generate a '405 - Method not allowed' response. Suspended = All 
        user and robots API calls will generate a '503 - Service Unavailable' response.
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -KillJobs <SwitchParameter>
        Forces the remaining active robot Jobs to be terminated instead of gracefully stopped. This parameter is only valid when entering the Suspended phase of a Maintenance Session
        
        Required?                    false
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Force <SwitchParameter>
        Bypasses all API validations and forces the UiPath Orchestrator service to enter the specifed Maintenance phase.
        
        Required?                    false
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
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
    
    ----------  EXAMPLE 1  ----------
    
    Start-UiPathMaintenance -Phase Draining
    
    Starts a Maintenance Mode session and puts the service in Draining mode.

    ----------  EXAMPLE 2  ----------
    
    Start-UiPathMaintenance -Phase Suspended -KillJobs
    
    Sets a current Maintenance Mode session to Suspended phase, forcing remaining jobs termination
    
RELATED LINKS



```
