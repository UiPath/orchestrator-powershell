using System;
using System.Linq;
using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    public abstract class EditCmdlet: AuthenticatedCmdlet 
    {
        protected void ProcessImpl<DtoType>(
            Func<DtoType> getItem,
            Action<DtoType> updateItem)
        {
            bool hasAction = false;

            DtoType dto = getItem();

            (dto, hasAction) = this.ApplySetParameters(dto);

            if (hasAction)
            {
                updateItem(dto);
            }
        }
    }
}
