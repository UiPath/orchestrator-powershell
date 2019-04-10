using System;
using System.Collections;
using System.ComponentModel;
using System.Management.Automation;
using System.Reflection;

namespace UiPath.PowerShell.Util
{
    public class UiPathTypeConverter : PSTypeConverter
    {
        public override bool CanConvertFrom(object sourceValue, Type destinationType)
        {
            return destinationType.Namespace == "UiPath.PowerShell.Models" &&
                (sourceValue is PSObject ||
                sourceValue is Hashtable);
        }

        public override bool CanConvertTo(object sourceValue, Type destinationType)
        {
            return false;
        }

        public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
        {
            if (sourceValue is PSObject)
            {
                return ConvertFromPSObject((PSObject)sourceValue, destinationType);
            }
            else if (sourceValue is Hashtable)
            {
                return ConvertFromHT((Hashtable)sourceValue, destinationType);
            }

            // Huh? We said that we can only convert the types we know...
            throw new NotImplementedException($"Need to parse: {sourceValue}");
        }

        private object ConvertFromHT(Hashtable sourceValue, Type destinationType)
        {
            return ConvertFromFN(destinationType, (propertyName) => sourceValue[propertyName]);
        }

        private static object ConvertFromPSObject(PSObject pso, Type destinationType)
        {
            return ConvertFromFN(destinationType, (propertyName) => pso.Properties[propertyName]?.Value);
        }

        private static object ConvertFromFN(Type destinationType, Func<string, object> fn)
        {
            var ctor = destinationType.GetConstructor(new Type[] { });
            var instance = ctor.Invoke(new object[] { });

            foreach(var p in destinationType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = fn(p.Name);
                if (value != null)
                {
                    if (!p.PropertyType.IsAssignableFrom(value.GetType()))
                    {
                        var converter = TypeDescriptor.GetConverter(p.PropertyType);
                        value = converter.ConvertFrom(value);
                    }
                    p.SetValue(instance, value);
                }
            }

            return instance;
        }

        public override object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
        {
            throw new NotImplementedException();
        }
    }
}
