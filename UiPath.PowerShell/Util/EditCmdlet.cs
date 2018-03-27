using System;
using System.Linq;
using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    public abstract class EditCmdlet<DtoType>: AuthenticatedCmdlet 
    {
        protected string MapParameterToProperty(string parameter)
        {
            return parameter;
        }


        protected void ProcessImpl(
            Func<DtoType> getItem,
            Action<DtoType> updateItem)
        {
            bool hasAction = false;

            DtoType dto = getItem();

            foreach (var p in this.GetType()
                .GetProperties()
                .Where(pi => pi.GetCustomAttributes(true)
                    .Where(o => o is SetParameterAttribute)
                    .Any()))
            {
                var value = p.GetValue(this);
                if (value != null)
                {
                    var propertyName = MapParameterToProperty(p.Name);
                    var dtoProperty = typeof(DtoType).GetProperty(propertyName);

                    // If there is a ValidateEnum attribute, parse the value as enum and assign the enum
                    ValidateEnumAttribute validateEnum = (ValidateEnumAttribute) p.GetCustomAttributes(true).Where(ca => ca is ValidateEnumAttribute).FirstOrDefault();
                    if (validateEnum != null)
                    {
                        value = Enum.Parse(validateEnum.EnumType, value.ToString());
                    }

                    if (value is SwitchParameter)
                    {
                        var switchValue = (SwitchParameter)value;
                        if (!switchValue.IsPresent)
                        {
                            continue;
                        }
                        value = switchValue.ToBool();
                    }

                    dtoProperty.SetValue(dto, value);
                    hasAction = true;
                }
            }

            if (hasAction)
            {
                updateItem(dto);
            }
        }
    }
}
