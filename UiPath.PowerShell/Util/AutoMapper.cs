using System;
using System.Linq;

namespace UiPath.PowerShell.Util
{
    internal static class AutoMapper
    {
        internal static TMapped To<TMapped>(this object from) where TMapped: new()
        {
            var mapped = new TMapped();

            var map = from.GetType().GetProperties()
                .Join(
                    typeof(TMapped).GetProperties(),
                    pf => pf.Name,
                    pt => pt.Name,
                    (pf, pt) => new { From = pf, To = pt });

            foreach(var p in map)
            {
                var fromType = Nullable.GetUnderlyingType(p.From.PropertyType) ?? p.From.PropertyType;
                var toType = Nullable.GetUnderlyingType(p.To.PropertyType) ?? p.To.PropertyType;

                if (toType.IsEnum)
                {
                    var s = p.From.GetValue(from)?.ToString();
                    if (s != null)
                    {
                        var e = Enum.Parse(toType, s);
                        p.To.SetValue(mapped, e);
                    }
                }
                else if (fromType.IsEnum && toType.IsAssignableFrom(typeof(string)))
                {
                    var s = p.From.GetValue(from)?.ToString();
                    p.To.SetValue(mapped, s);
                }
                else if (fromType.IsAssignableFrom(toType))
                {
                    var v = p.From.GetValue(from);
                    p.To.SetValue(mapped, v);
                }
            }

            return mapped;
        }
    }
}
