```PowerShell

NAME
    Edit-UiPathAsset
    
SYNOPSIS
    
    
SYNTAX
    Edit-UiPathAsset [-Asset] <Asset> -TextValue <string> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> -IntValue <int> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> -DBConnectionString <string> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> -HttpConnectionString <string> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> -BoolValue <bool> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> -KeyValueList <Hashtable> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> -WindowsCredential <PSCredential> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> -Credential <PSCredential> [-AuthToken <AuthToken>] [<CommonParameters>]
    
    Edit-UiPathAsset [-Asset] <Asset> [-AddRobotValues <AssetRobotValue[]>] [-AuthToken <AuthToken>] 
    [-RemoveRobotIdValues <long[]>] [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Asset <Asset>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       true (ByValue)
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
        
    -AddRobotValues <AssetRobotValue[]>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -RemoveRobotIdValues <long[]>
        
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
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
INPUTS
        UiPath.PowerShell.Models.AssetAn UiPath Orchestrator asset
    
    
    
OUTPUTS
    
    
RELATED LINKS



```
