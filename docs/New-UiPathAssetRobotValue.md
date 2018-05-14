```PowerShell

NAME
    New-UiPathAssetRobotValue
    
SYNOPSIS
    Used to create a robot asset value
    
    
SYNTAX
    New-UiPathAssetRobotValue -RobotId <long> -TextValue <string> [<CommonParameters>]
    
    New-UiPathAssetRobotValue -DBConnectionString <string> -RobotId <long> [<CommonParameters>]
    
    New-UiPathAssetRobotValue -HttpConnectionString <string> -RobotId <long> [<CommonParameters>]
    
    New-UiPathAssetRobotValue -IntValue <int> -RobotId <long> [<CommonParameters>]
    
    New-UiPathAssetRobotValue -BoolValue <bool> -RobotId <long> [<CommonParameters>]
    
    New-UiPathAssetRobotValue -KeyValueList <Hashtable> -RobotId <long> [<CommonParameters>]
    
    New-UiPathAssetRobotValue -Credential <PSCredential> -RobotId <long> [<CommonParameters>]
    
    New-UiPathAssetRobotValue -RobotId <long> -WindowsCredential <PSCredential> [<CommonParameters>]
    
    
DESCRIPTION
    This cmdlet produces no effect on the Orchestrator. Use the returned Robot Asset Value object with the 
    Add-UiPathAsset -RobotValues to actually create the robot asset value.
    

PARAMETERS
    -RobotId <long>
        
        Required?                    true
        Position?                    named
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -TextValue <string>
        
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
        
    -IntValue <int>
        
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
        
    -Credential <PSCredential>
        
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
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
INPUTS
    
OUTPUTS
    
    ----------  EXAMPLE 1  ----------
    
    $robotId2Value = New-UiPathAssetRobotValue -RobotId 2 -TextValue SomeValue
                $robotId4Value = New-UiPathAssetRobotValue -RobotId 4 -TextValue AnotherValue
                Add-UiPath MyAsset -RobotValues $robotId2Value,$robotId4Value
    
    Creates two robot asset values and creates an asset with these two values.
    
    Note that all robot values for an asset must have the same type.
    
RELATED LINKS



```
