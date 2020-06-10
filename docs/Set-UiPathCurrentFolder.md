```PowerShell

NAME
    Set-UiPathCurrentFolder
    
SYNOPSIS
    Changes the current Folder under which all other cmdlets are evaluated
    
    
SYNTAX
    Set-UiPathCurrentFolder [-Folder] <Folder> [-AuthToken <AuthToken>] [-RequestTimeout <int>] [<CommonParameters>]
    
    Set-UiPathCurrentFolder [-FolderPath] <string> [-AuthToken <AuthToken>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Folder <Folder>
        
        Required?                    true
        Position?                    1
        Default value                
        Accept pipeline input?       true (ByValue)
        Accept wildcard characters?  false
        
    -FolderPath <string>
        
        Required?                    true
        Position?                    1
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
    UiPath.PowerShell.Models.Folder
    
    
OUTPUTS
    
    ----------  EXAMPLE 1  ----------
    
    Set-UiPathCurrentFolder Some/Folder/Path
    
    
RELATED LINKS



```
