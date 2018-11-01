using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Util
{
    internal static class HashtableExtenssions
    {
        public static List<CustomKeyValuePair> ToKeyList(this Hashtable ht)
        {
            return ht.Cast<DictionaryEntry>().Select(de => new CustomKeyValuePair
            {
                Key = de.Key.ToString(),
                Value = de.Value.ToString()
            }).ToList();
        }
    }
}
