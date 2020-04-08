```PowerShell

NAME
    Edit-UiPathFolder
    
SYNOPSIS
    
    
SYNTAX
    Edit-UiPathFolder [-Folder] <Folder> [-DisplayName] <string> [-AuthToken <AuthToken>] [-Description <string>] 
    [-ParentId <long>] [-PermissionModel {InheritFromTenant | FineGrained}] [-ProvisionType {Manual | Automatic}] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    Edit-UiPathFolder [-Id] <long> [-DisplayName] <string> [-AuthToken <AuthToken>] [-Description <string>] [-ParentId 
    <long>] [-PermissionModel {InheritFromTenant | FineGrained}] [-ProvisionType {Manual | Automatic}] 
    [-RequestTimeout <int>] [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Folder <Folder>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       true (ByValue)
        Accept wildcard characters?  false
        
    -Id <long>
        
        Required?                    true
        Position?                    0
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -DisplayName <string>
        
        Required?                    true
        Position?                    1
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Description <string>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -ProvisionType <FolderDtoProvisionType>
        Possible values: Manual, Automatic
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -PermissionModel <FolderDtoPermissionModel>
        Possible values: InheritFromTenant, FineGrained
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -ParentId <long>
        
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
    UiPath.PowerShell.Models.Folder
    
    
OUTPUTS
    
    
RELATED LINKS



```
