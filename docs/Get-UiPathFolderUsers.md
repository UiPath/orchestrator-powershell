```PowerShell

NAME
    Get-UiPathFolderUsers
    
SYNTAX
    Get-UiPathFolderUsers [-Folder] <Folder> [-IncludeInherited <bool>] [-Paging] [-AuthToken <AuthToken>] 
    [-RequestTimeout <int>]  [<CommonParameters>]
    
    Get-UiPathFolderUsers -Id <long> [-IncludeInherited <bool>] [-Paging] [-AuthToken <AuthToken>] [-RequestTimeout 
    <int>]  [<CommonParameters>]
    
    
PARAMETERS
    -AuthToken <AuthToken>
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    -Folder <Folder>
        
        Required?                    true
        Position?                    0
        Accept pipeline input?       true (ByValue)
        Parameter set name           Folder
        Aliases                      None
        Dynamic?                     false
        
    -Id <long>
        
        Required?                    true
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           Id
        Aliases                      None
        Dynamic?                     false
        
    -IncludeInherited <bool>
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    -Paging
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    -RequestTimeout <int>
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (https:/go.microsoft.com/fwlink/?LinkID=113216). 
    
    
INPUTS
    UiPath.PowerShell.Models.Folder
    
    
OUTPUTS
    System.Object
    
ALIASES
    None
    

REMARKS
    None



```
