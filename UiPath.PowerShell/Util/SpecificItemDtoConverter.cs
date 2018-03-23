using System;
using Newtonsoft.Json;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Util
{
    internal class SpecificItemDtoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(QueueItemSpecificContent));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var itemData = value as QueueItemSpecificContent;

            writer.WriteStartObject();
            foreach (var kv in itemData.DynamicProperties)
            {
                writer.WritePropertyName(kv.Key);
                writer.WriteValue(kv.Value);
            }
            writer.WriteEndObject();
        }
    }
}
