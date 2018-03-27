using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    public abstract class UiPathCmdlet: PSCmdlet
    {
        private bool Ignored = BindingResolver.Ignored;
    }
}
