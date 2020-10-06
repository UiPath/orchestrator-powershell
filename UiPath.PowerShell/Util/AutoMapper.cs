using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UiPath.PowerShell.Util
{
    internal static class AutoMapper
    {
        internal static TMapped To<TMapped>(this object from) where TMapped : new()
        {
            return (TMapped)from.To(typeof(TMapped), () => new TMapped());
        }

        private static object To(this object from, Type tMapped)
        {
            var ctor = tMapped.GetConstructor(Array.Empty<Type>());
            return To(from, tMapped, () => ctor.Invoke(BindingFlags.CreateInstance, Array.Empty<object>()));
        }

        private static Type EnumerableElementType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            if (underlyingType.IsArray)
            {
                return underlyingType.GetElementType();
            }

            if (type.IsGenericType)
            {
                return underlyingType.GetGenericArguments()[0];
            }

            throw new Exception($"Don't know how to extract collection element type from {underlyingType.Name}");
        }

        private static object To(this object from, Type tMapped, Func<object> ctor)
        {
            if (from is Hashtable hashtable)
            {
                return FromHashtable(hashtable, tMapped, ctor);
            }

            if (typeof(Hashtable).IsAssignableFrom(tMapped))
            {
                return from.ToHashtable();
            }

            var mapped = ctor();

            var map = from.GetType().GetProperties()
                .Join(
                    tMapped.GetProperties(),
                    pf => pf.Name,
                    pt => pt.Name,
                    (pf, pt) => new { From = pf, To = pt });

            foreach(var p in map)
            {
                try
                {
                    var fromType = Nullable.GetUnderlyingType(p.From.PropertyType) ?? p.From.PropertyType;
                    var toType = Nullable.GetUnderlyingType(p.To.PropertyType) ?? p.To.PropertyType;

                    if (toType.IsArray)
                    {
                        var toElementType = EnumerableElementType(toType);
                        var fromValue = p.From.GetValue(from);
                        var toValue = ToEnumerable(fromValue, toElementType);
                        p.To.SetValue(mapped, ToArray(toValue, toElementType));
                    }
                    else if (toType.IsEnum)
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
                    UiPathCmdlet.DebugMessage($"{from?.GetType().Name}->{tMapped}:{p.From.Name}: {e.GetType().Name}: {e.Message}");
                }
            }

            return mapped;
        }

        internal static TMapped FromHashtable<TMapped>(this Hashtable from) where TMapped : new()
        {
            return (TMapped)from.FromHashtable(typeof(TMapped), () => new TMapped());
        }

        private static Array ToArray(IEnumerable values, Type elementType)
        {
            if (values == null)
            {
                return null;
            }

            var arr = Array.CreateInstance(elementType, values.Cast<object>().Count());

            int i = 0;
            foreach(var value in values)
            {
                arr.SetValue(value, i);
                ++i;
            }

            return arr;
        }

        private static IEnumerable ToEnumerable(object fromValues, Type toType)
        {
            if (fromValues == null)
            {
                return null;
            }

            var list = new List<object>();

            if (fromValues is IEnumerable enumValues)
            {
                foreach (var fromValue in enumValues)
                {
                    var toValue = fromValue.To(toType);
                    list.Add(toValue);
                }
            }

            return list;
        }

        private static object FromHashtable(this Hashtable from, Type tMapped, Func<object> ctor)
        {
            if (from == null)
            {
                return default;
            }

            var props = tMapped.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var to = ctor();
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
