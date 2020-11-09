```PowerShell

NAME
    Edit-UiPathMachine
    
SYNOPSIS
    
    
SYNTAX
    Edit-UiPathMachine [-Machine] <Machine> [-AuthToken <AuthToken>] [-HeadlessSlots <int>] [-Name <string>] 
    [-NonProductionSlots <int>] [-RequestTimeout <int>] [-TestAutomationSlots <int>] [-Type {Standard | Template}] 
    [-UnattendedSlots <int>] [<CommonParameters>]
    
    Edit-UiPathMachine -Id <long> [-AuthToken <AuthToken>] [-HeadlessSlots <int>] [-Name <string>] 
    [-NonProductionSlots <int>] [-RequestTimeout <int>] [-TestAutomationSlots <int>] [-Type {Standard | Template}] 
    [-UnattendedSlots <int>] [<CommonParameters>]
    
    
DESCRIPTION
    

PARAMETERS
    -Machine <Machine>
        
        Required?                    true
        Position?                    0
        Default value                
        Accept pipeline input?       true (ByValue)
        Accept wildcard characters?  false
        
    -Id <long>
        
        Required?                    true
        Position?                    named
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Name <string>
        
        Required?                    false
        Position?                    named
        Default value                
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -NonProductionSlots <int>
        
        Required?                    false
        Position?                    named
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -Type <MachineDtoType>
        Possible values: Standard, Template
        
        Required?                    false
        Position?                    named
        Default value                Standard
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -UnattendedSlots <int>
        
        Required?                    false
        Position?                    named
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -TestAutomationSlots <int>
        
        Required?                    false
        Position?                    named
        Default value                0
        Accept pipeline input?       false
        Accept wildcard characters?  false
        
    -HeadlessSlots <int>
        
        Required?                    false
        Position?                    named
        Default value                0
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
    UiPath.PowerShell.Models.Machine
    
    
OUTPUTS
    
    
RELATED LINKS



```
