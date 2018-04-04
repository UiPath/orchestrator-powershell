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
    

RELATED LINKS

REMARKS
    To see the examples, type: "get-help Add-UiPathAsset -examples".
    For more information, type: "get-help Add-UiPathAsset -detailed".
    For technical information, type: "get-help Add-UiPathAsset -full".



```
