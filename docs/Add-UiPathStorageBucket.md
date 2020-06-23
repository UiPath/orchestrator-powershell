```PowerShell

NAME
    Add-UiPathStorageBucket
    
SYNOPSIS
    
    
SYNTAX
    Add-UiPathStorageBucket [-Name] <string> [-AuditReadAccess <SwitchParameter>] [-AuthToken <AuthToken>] 
    [-Description <string>] [-ReadOnly <SwitchParameter>] [-RequestTimeout <int>] [<CommonParameters>]
    
    Add-UiPathStorageBucket [-Name] <string> -FileSystem <SwitchParameter> -Path <string> [-AuditReadAccess 
    <SwitchParameter>] [-AuthToken <AuthToken>] [-Description <string>] [-ReadOnly <SwitchParameter>] [-RequestTimeout 
    <int>] [<CommonParameters>]
    
    Add-UiPathStorageBucket [-Name] <string> -AccessKey <string> -Aws <SwitchParameter> -EndpointRegion <string> 
    -S3BucketName <string> -SecretKey <string> [-AuditReadAccess <SwitchParameter>] [-AuthToken <AuthToken>] 
    [-CredentialStore <CredentialStore>] [-Description <string>] [-ReadOnly <SwitchParameter>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    Add-UiPathStorageBucket [-Name] <string> -AccountKey <string> -AccountName <string> -Azure <SwitchParameter> 
    -ContainerName <string> -EndpointSuffix <string> [-AuditReadAccess <SwitchParameter>] [-AuthToken <AuthToken>] 
    [-CredentialStore <CredentialStore>] [-Description <string>] [-ReadOnly <SwitchParameter>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    Add-UiPathStorageBucket [-Name] <string> -AccessKey <string> -ContainerName <string> -MinIO <SwitchParameter> 
    -SecretKey <string> -Server <string> [-AuditReadAccess <SwitchParameter>] [-AuthToken <AuthToken>] 
    [-CredentialStore <CredentialStore>] [-Description <string>] [-ReadOnly <SwitchParameter>] [-RequestTimeout <int>] 
    [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Name <string>
        
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
        
    -FileSystem <SwitchParameter>
        
        Required?                    true
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Path <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Aws <SwitchParameter>
        
        Required?                    true
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -S3BucketName <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -EndpointRegion <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -AccessKey <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -SecretKey <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Azure <SwitchParameter>
        
        Required?                    true
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -ContainerName <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -AccountName <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -AccountKey <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -EndpointSuffix <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -MinIO <SwitchParameter>
        
        Required?                    true
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Server <string>
        
        Required?                    true
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -CredentialStore <CredentialStore>
        
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
    
    
RELATED LINKS



```
