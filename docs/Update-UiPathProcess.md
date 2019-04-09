```PowerShell

NAME
    Update-UiPathProcess
    
SYNOPSIS
    
    
SYNTAX
    Update-UiPathProcess -Id <long> -PackageVersion <string> [-AuthToken <AuthToken>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    Update-UiPathProcess -Id <long> -Package <Package> [-AuthToken <AuthToken>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    Update-UiPathProcess -Id <long> -Latest <SwitchParameter> [-AuthToken <AuthToken>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    Update-UiPathProcess -Id <long> -Rollback <SwitchParameter> [-AuthToken <AuthToken>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    Update-UiPathProcess [-Process] <Process> -PackageVersion <string> [-AuthToken <AuthToken>] [-RequestTimeout 
    <int>] [<CommonParameters>]
    
    Update-UiPathProcess [-Process] <Process> -Package <Package> [-AuthToken <AuthToken>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    Update-UiPathProcess [-Process] <Process> -Latest <SwitchParameter> [-AuthToken <AuthToken>] [-RequestTimeout 
    <int>] [<CommonParameters>]
    
    Update-UiPathProcess [-Process] <Process> -Rollback <SwitchParameter> [-AuthToken <AuthToken>] [-RequestTimeout 
    <int>] [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Id <long>
        
        Required?                    true
        Position?                    named
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Process <Process>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       true (ByValue)
        Accept wildcard characters?  false
        
    -PackageVersion <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Package <Package>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Latest <SwitchParameter>
        
        Required?                    true
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Rollback <SwitchParameter>
        
        Required?                    true
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
    UiPath.PowerShell.Models.Process
    
    
OUTPUTS
    
    
RELATED LINKS



```
