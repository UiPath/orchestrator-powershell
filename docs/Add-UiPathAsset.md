```PowerShell

NAME
    Add-UiPathAsset
    
SYNOPSIS
    Adds an Asset into Orchestrator
    
    
SYNTAX
    Add-UiPathAsset [-Name] <string> -TextValue <string> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -IntValue <int> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -DBConnectionString <string> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -HttpConnectionString <string> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -BoolValue <bool> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -KeyValueList <Hashtable> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -WindowsCredential <PSCredential> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -Credential <PSCredential> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -RobotValues <Hashtable> -ValueType <string> [-AuthToken <AuthToken>] 
    [<CommonParameters>]
    
    
DESCRIPTION
    This cmdlet can add global asset value or per-robot asset values.
    

PARAMETERS
    -Name <string>
        The asset name.
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -TextValue <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -IntValue <int>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -DBConnectionString <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -HttpConnectionString <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -BoolValue <bool>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -KeyValueList <Hashtable>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -WindowsCredential <PSCredential>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Credential <PSCredential>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -ValueType <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -RobotValues <Hashtable>
        
        Required?                    true
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
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
INPUTS
    
OUTPUTS
    
    
RELATED LINKS



```
