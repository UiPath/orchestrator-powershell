```PowerShell

NAME
    Add-UiPathProcessSchedule
    
SYNOPSIS
    Adds a process schedule into Orchestrator
    
    
SYNTAX
    Add-UiPathProcessSchedule [-Name] <string> -AllRobots <SwitchParameter> -Process <Process> -StartProcessCron 
    <string> [-AuthToken <AuthToken>] [-RequestTimeout <int>] [-StopAfterMinutes <int>] [-StopStrategy <string>] 
    [-TimeZoneId <string>] [<CommonParameters>]
    
    Add-UiPathProcessSchedule [-Name] <string> -Process <Process> -RobotCount <int> -StartProcessCron <string> 
    [-AuthToken <AuthToken>] [-RequestTimeout <int>] [-StopAfterMinutes <int>] [-StopStrategy <string>] [-TimeZoneId 
    <string>] [<CommonParameters>]
    
    Add-UiPathProcessSchedule [-Name] <string> -Process <Process> -Robots <List`1> -StartProcessCron <string> 
    [-AuthToken <AuthToken>] [-RequestTimeout <int>] [-StopAfterMinutes <int>] [-StopStrategy <string>] [-TimeZoneId 
    <string>] [<CommonParameters>]
    
    Add-UiPathProcessSchedule [-Name] <string> -Process <Process> -Queue <QueueDefinition> -StartProcessCron <string> 
    [-AuthToken <AuthToken>] [-ItemsActivationThreshold <long>] [-ItemsPerJobActivationTarget <long>] 
    [-MaxJobsForActivation <int>] [-RequestTimeout <int>] [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Name <string>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -StartProcessCron <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Process <Process>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -AllRobots <SwitchParameter>
        
        Required?                    true
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -RobotCount <int>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Robots <List`1>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -TimeZoneId <string>
        
        Required?                    false
        Position?                    named
        Default value                UTC
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -StopAfterMinutes <int>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -StopStrategy <string>
        
        Required?                    false
        Position?                    named
        Default value                Kill
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Queue <QueueDefinition>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -ItemsActivationThreshold <long>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -ItemsPerJobActivationTarget <long>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -MaxJobsForActivation <int>
        
        Required?                    false
        Position?                    named
        Default value                
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
    
    
RELATED LINKS



```
