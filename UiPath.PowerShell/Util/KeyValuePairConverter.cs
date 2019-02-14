using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Util
{
    internal class KeyValuePairConverter: JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ODataResponseListKeyValuePairStringString).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var jContext = jObject.GetValue("@odata.context", StringComparison.InvariantCultureIgnoreCase);
            var jKeys = jObject.GetValue("Keys", StringComparison.InvariantCultureIgnoreCase);
            var jValues = jObject.GetValue("Values", StringComparison.InvariantCultureIgnoreCase);

            var keys = jKeys as JArray;
            var values = jValues as JArray;

            if (keys == null || values == null || keys.Count != values.Count)
            {
                return null;
            }

            var kv = new List<KeyValuePairStringString>();
            for (int i=0; i< keys.Count; ++i)
            {
                kv.Add(new KeyValuePairStringString(
                    keys[i].Value<string>(),
                    values[i].Value<string>()));
            }

            var resp = new ODataResponseListKeyValuePairStringString(
                jContext.Value<string>(),
                kv);
            serializer.Populate(jObject.CreateReader(), resp);
            return resp;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
