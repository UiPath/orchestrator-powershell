using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace UiPath.PowerShell.Util
{
    internal static class AutoMapper
    {
        internal static TMapped To<TMapped>(this object from) where TMapped: new()
        {
            if (from is Hashtable hashtable)
            {
                return FromHashtable<TMapped>(hashtable);
            }

            var mapped = new TMapped();

            var map = from.GetType().GetProperties()
                .Join(
                    typeof(TMapped).GetProperties(),
                    pf => pf.Name,
                    pt => pt.Name,
                    (pf, pt) => new { From = pf, To = pt });

            foreach(var p in map)
            {
                try
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
                    else if (typeof(Hashtable).IsAssignableFrom(toType))
                    {
                        var ht = p.From.GetValue(from).ToHashtable();
                        p.To.SetValue(mapped, ht);
                    }
                    else if (fromType.IsAssignableFrom(toType))
                    {
                        var v = p.From.GetValue(from);
                        p.To.SetValue(mapped, v);
                    }
                }
                catch (Exception e)
                {
                    UiPathCmdlet.DebugMessage($"{from?.GetType().Name}->{typeof(TMapped)}:{p.From.Name}: {e.GetType().Name}: {e.Message}");
                }
            }

            return mapped;
        }

        internal static TMapped FromHashtable<TMapped>(this Hashtable from) where TMapped: new()
        {
            if (from == null)
            {
                return default;
            }

            var props = typeof(TMapped).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var to = new TMapped();
            foreach (var pi in props)
            {
                if (from.ContainsKey(pi.Name))
                {
                    var value = from[pi.Name];
                    pi.SetValue(to, value);
                }
            }
            return to;
        }

        internal static Hashtable ToHashtable(this object from)
        {
            if (from == null)
            {
                return null;
            }

            var props = from.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var ht = new Hashtable();

            foreach(var pi in  props)
            {
                var v = pi.GetValue(from);
                if (v == null)
                {
                    continue;
                }

                var vType = v.GetType();

                if (vType.IsClass && !(v is string))
                {
                    ht.Add(pi.Name, v.ToHashtable());
                }
                else
                {
                    ht.Add(pi.Name, v);
                }
            }

            return ht;
        }
    }
}
