using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    public abstract class UiPathCmdlet: Cmdlet
    {
        private bool Ignored = BindingResolver.Ignored;
    }
}
