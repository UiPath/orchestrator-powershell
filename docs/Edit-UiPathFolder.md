```PowerShell

NAME
    Edit-UiPathFolder
    
SYNTAX
    Edit-UiPathFolder [-Folder] <Folder> [-DisplayName] <string> [-Description <string>] [-ProvisionType {Manual | 
    Automatic}] [-PermissionModel {InheritFromTenant | FineGrained}] [-ParentId <long>] [-AuthToken <AuthToken>] 
    [-RequestTimeout <int>]  [<CommonParameters>]
    
    Edit-UiPathFolder [-Id] <long> [-DisplayName] <string> [-Description <string>] [-ProvisionType {Manual | 
    Automatic}] [-PermissionModel {InheritFromTenant | FineGrained}] [-ParentId <long>] [-AuthToken <AuthToken>] 
    [-RequestTimeout <int>]  [<CommonParameters>]
    
    
PARAMETERS
    -AuthToken <AuthToken>
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    -Description <string>
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    -DisplayName <string>
        
        Required?                    true
        Position?                    1
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
        Position?                    0
        Accept pipeline input?       false
        Parameter set name           Id
        Aliases                      None
        Dynamic?                     false
        
    -ParentId <long>
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    -PermissionModel <FolderDtoPermissionModel>
        
        Required?                    false
        Position?                    Named
        Accept pipeline input?       false
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    -ProvisionType <FolderDtoProvisionType>
        
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
