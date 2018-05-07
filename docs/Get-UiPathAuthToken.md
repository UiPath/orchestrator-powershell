```PowerShell

NAME
    Get-UiPathAuthToken
    
SYNOPSIS
    Obtains an UiPath authentication token
    
    
    ----------  EXAMPLE 1  ----------
    
    Get-UiPathAuthToken -URL https://platform.uipath.com -Username <myuser> -Password <mypassword>
    
    Connect to UiPath public platform Orchestrator, using user name and password.
    ----------  EXAMPLE 2  ----------
    
    Get-UiPathAuthToken -URL https://platform.uipath.com -Username <myuser> -Password <mypassword> -Session
    
    Connect to UiPath public platform Orchestrator, using user name and password and save the token for the current 
    session.
    ----------  EXAMPLE 3  ----------
    
    Get-UiPathAuthToken -URL https://uipath.corpnet -WindowsCredentials -Session
    
    Connect to a private Orchestrator with Windows enabled, using current Windows credentials and save the token for 
    current session.



```
