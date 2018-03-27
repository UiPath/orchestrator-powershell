using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ValidateEnumAttribute : ValidateEnumeratedArgumentsAttribute
    {
        private string[] _enumValues;

        public Type EnumType { get; private set; }

        public ValidateEnumAttribute(Type type)
        {
            _enumValues = Enum.GetNames(type);
            EnumType = type;
        }

        public IList<string> ValidValues => _enumValues.ToList();

        protected override void ValidateElement(object element)
        {
            if (!_enumValues.Contains(element.ToString()))
            {
                throw new ValidationMetadataException($"The argument \"{element}\" does not belong to the set \"{String.Join(",", _enumValues)}\" specified by the ValidateEnum attribute. Supply an argument that is in the set and then try the command again");
            }
        }
    }
}
