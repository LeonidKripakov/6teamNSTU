using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
public class ICardConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(ICard).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject? jsonObject = JObject.Load(reader);

        // Extract the $type property to determine the actual type
        string? typeName = (string?)jsonObject?["$type"];
        Type? targetType = typeName != null ? Type.GetType(typeName) : null;

        // Deserialize into the specific type
        return targetType != null ? JsonConvert.DeserializeObject(jsonObject?.ToString(), targetType) ?? Activator.CreateInstance(targetType) : null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
}
