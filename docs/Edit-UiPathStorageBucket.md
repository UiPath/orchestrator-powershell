```PowerShell

NAME
    Edit-UiPathStorageBucket
    
SYNOPSIS
    
    
SYNTAX
    Edit-UiPathStorageBucket -Id <long> [-AuditReadAccess <SwitchParameter>] [-AuthToken <AuthToken>] [-Description 
    <string>] [-ReadOnly <SwitchParameter>] [-RequestTimeout <int>] [<CommonParameters>]
    
    Edit-UiPathStorageBucket [-Bucket] <Bucket> [-AuditReadAccess <SwitchParameter>] [-AuthToken <AuthToken>] 
    [-Description <string>] [-ReadOnly <SwitchParameter>] [-RequestTimeout <int>] [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Id <long>
        
        Required?                    true
        Position?                    named
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Bucket <Bucket>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       true (ByValue)
        Accept wildcard characters?  false
        
    -Description <string>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -ReadOnly <SwitchParameter>
        
        Required?                    false
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -AuditReadAccess <SwitchParameter>
        
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
    UiPath.PowerShell.Models.Bucket
    
    
OUTPUTS
    
    
RELATED LINKS



```
