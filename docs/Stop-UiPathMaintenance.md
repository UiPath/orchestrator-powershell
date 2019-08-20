```PowerShell

NAME
    Stop-UiPathMaintenance
    
SYNOPSIS
    Stops a Maintenance session
    
    
SYNTAX
    Stop-UiPathMaintenance [-AuthToken <AuthToken>] [-RequestTimeout <int>] [<CommonParameters>]
    
    
DESCRIPTION
    This cmdlet will end the current Maintenance session for UiPath Orchestrator service.
    

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
    
    ----------  EXAMPLE 1  ----------
    
    Stop-UiPathMaintenance
    
    Stops the current Maintenance Mode session and puts the service back in Online mode.
    
RELATED LINKS



```
