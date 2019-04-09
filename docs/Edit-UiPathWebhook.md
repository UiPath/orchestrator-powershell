```PowerShell

NAME
    Edit-UiPathWebhook
    
SYNOPSIS
    
    
SYNTAX
    Edit-UiPathWebhook [-Id] <long> [-Webhook] <Webhook> [-AllEvents <SwitchParameter>] [-AllowInsecureSsl 
    <SwitchParameter>] [-AuthToken <AuthToken>] [-Enabled <bool>] [-RequestTimeout <int>] [-Secret <string>] [-Url 
    <string>] [<CommonParameters>]
    
    Edit-UiPathWebhook [-Id] <long> [-Webhook] <Webhook> [-AllowInsecureSsl <SwitchParameter>] [-AuthToken 
    <AuthToken>] [-Enabled <bool>] [-Events <string[]>] [-RequestTimeout <int>] [-Secret <string>] [-Url <string>] 
    [<CommonParameters>]
    
    Edit-UiPathWebhook [-Id] <long> [-AllEvents <SwitchParameter>] [-AllowInsecureSsl <SwitchParameter>] [-AuthToken 
    <AuthToken>] [-Enabled <bool>] [-Events <string[]>] [-RequestTimeout <int>] [-Secret <string>] [-Url <string>] 
    [<CommonParameters>]
    
    Edit-UiPathWebhook [-Webhook] <Webhook> [-AllEvents <SwitchParameter>] [-AllowInsecureSsl <SwitchParameter>] 
    [-AuthToken <AuthToken>] [-Enabled <bool>] [-Events <string[]>] [-RequestTimeout <int>] [-Secret <string>] [-Url 
    <string>] [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Id <long>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Webhook <Webhook>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       true (ByValue)
        Accept wildcard characters?  false
        
    -Url <string>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Enabled <bool>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Secret <string>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -AllowInsecureSsl <SwitchParameter>
        
        Required?                    false
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -AllEvents <SwitchParameter>
        
        Required?                    false
        Position?                    named
        Default value                False
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Events <string[]>
        
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
    UiPath.PowerShell.Models.Webhook
    
    
OUTPUTS
    
    
RELATED LINKS



```
