```PowerShell

NAME
    Add-UiPathAsset
    
SYNOPSIS
    Adds an Asset into Orchestrator
    
    
SYNTAX
    Add-UiPathAsset [-Name] <string> -TextValue <string> [-AuthToken <AuthToken>] [-Description <string>] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -IntValue <int> [-AuthToken <AuthToken>] [-Description <string>] [-RequestTimeout 
    <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -DBConnectionString <string> [-AuthToken <AuthToken>] [-Description <string>] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -HttpConnectionString <string> [-AuthToken <AuthToken>] [-Description <string>] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -BoolValue <bool> [-AuthToken <AuthToken>] [-Description <string>] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -KeyValueList <Hashtable> [-AuthToken <AuthToken>] [-Description <string>] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -WindowsCredential <PSCredential> [-AuthToken <AuthToken>] [-Description 
    <string>] [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -Credential <PSCredential> [-AuthToken <AuthToken>] [-Description <string>] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathAsset [-Name] <string> -RobotValues <AssetRobotValue[]> [-AuthToken <AuthToken>] [-Description <string>] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    
DESCRIPTION
    This cmdlet can add global asset value or per-robot asset values.
    
    The asset type is deduced from the parameter set used.
    
    To create per robot values, use New-UiPathAssetRobotValue
    

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
        
    -RobotValues <AssetRobotValue[]>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Description <string>
        
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
    
    ----------  EXAMPLE 1  ----------
    
    Add-UiPathAsset AGlobalTextAsset -TextValue SomeText
    
    Creates a global asset of type text with the value SomeText.
    ----------  EXAMPLE 2  ----------
    
    $creds = Get-Credential
                Add-UiPathAsset AGlobalWindowsCredentialAsset -WindowsCredential $creds
    
    Creates a global asset of type text with the value SomeText.
    
RELATED LINKS



```
