using System;

namespace UiPath.PowerShell.Util
{
    public static class EnumExtensions
    {
        public static TEnum Cast<TEnum>(this Enum val) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(val.ToString(), out var result))
            {
                return result;
            }

            throw new Exception($"Cannot cast value {val} ({val.GetType().Name}) to type {typeof(TEnum).Name}");
        }
    }
}
