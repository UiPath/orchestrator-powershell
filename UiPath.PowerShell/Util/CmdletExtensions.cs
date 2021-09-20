using Microsoft.Rest;
using System;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Threading.Tasks;

namespace UiPath.PowerShell.Util
{
    internal static class CmdletExtensions
    {
        internal static long? GetDefaultCredentialStore(this AuthenticatedCmdlet cmdlet, string resourceType)
        {
            var id = cmdlet.HandleHttpResponseException(() => cmdlet.Api_19_10.CredentialStores.GetDefaultStoreForResourceTypeByResourcetypeWithHttpMessagesAsync(resourceType));
            return id.Value;
        }

        internal static (T, bool) ApplySetParameters<T>(this PSCmdlet cmdlet, T dto)
        {
            var cmdletParams = cmdlet.GetType()
                .GetProperties()
                .Select(pi => new { Property = pi, SetParameter = pi.GetCustomAttribute<SetParameterAttribute>()})
                .Where(p => p.SetParameter != null);
            var dtoProps = typeof(T).GetProperties();

            var boundParams = cmdlet.MyInvocation.BoundParameters;

            var toApply = cmdletParams
                    .Join(dtoProps, cp => cp.SetParameter.DtoProperty ?? cp.Property.Name, pi => pi.Name, (cp, pi) => new { Parameter = cp, DtoProperty = pi })
                    .Join(boundParams, cp => cp.Parameter.Property.Name, kv => kv.Key, (cp, kv) => new { cp.Parameter, cp.DtoProperty, kv.Value });

            var anyChanges = false;

            foreach(var p in toApply)
            {
                var value = p.Value;

                var dtoPropertyType = p.DtoProperty.PropertyType;
                if (dtoPropertyType.IsGenericType && dtoPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    dtoPropertyType = Nullable.GetUnderlyingType(dtoPropertyType);
                }

                //Convert SwitchParameters to bool value
                if (value is SwitchParameter switchValue)
                {
                    value = switchValue.ToBool();
                }

                // Convert strings to enums 
                if (dtoPropertyType.IsEnum && value is string stringValue)
                {
                    value = Enum.Parse(p.DtoProperty.PropertyType, stringValue);
                }

                // Convert flag enums (turn on-off bits)
                if (dtoPropertyType.IsEnum && value is bool boolValue)
                {
                    var names = Enum.GetNames(dtoPropertyType);
                    var bits = Enum.GetValues(dtoPropertyType);
                    var enumValueType = dtoPropertyType.GetEnumUnderlyingType();

                    var flagIndex = Array.IndexOf(names, p.Parameter.Property.Name);
                    var flagValue = bits.GetValue(flagIndex);
                    var currentValue = p.DtoProperty.GetValue(dto);

                    // For arithmetic we need numerics
                    var longValue = Convert.ToInt64(currentValue);
                    var bitValue = Convert.ToInt64(flagValue);

                    if (boolValue)
                    {
                        longValue |= bitValue; // turn the flag on
                    }
                    else
                    {
                        longValue &= ~bitValue; //turn the flag off
                    }

                    value = Enum.ToObject(dtoPropertyType, longValue);
                }

                p.DtoProperty.SetValue(dto, value);
                anyChanges = true;
            }

            return (dto, anyChanges);
        }
    }
}
