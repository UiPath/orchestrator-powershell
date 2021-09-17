using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Util
{
    internal static class HashtableExtensions
    {
        public static List<CustomKeyValuePair> ToKeyList(this Hashtable ht)
        {
            return ht.Cast<DictionaryEntry>().Select(de => new CustomKeyValuePair
            {
                Key = de.Key.ToString(),
                Value = de.Value.ToString()
            }).ToList();
        }

        public static Hashtable ToHashtable<TKey, TValue>(this IDictionary<TKey, TValue> dict) => new Hashtable((IDictionary)dict);
    }
}
