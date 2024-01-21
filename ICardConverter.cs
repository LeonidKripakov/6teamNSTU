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
        JObject jsonObject = JObject.Load(reader);

        // Extract the $type property to determine the actual type
        string typeName = (string)jsonObject["$type"];
        Type targetType = Type.GetType(typeName);

        // Deserialize into the specific type
        return JsonConvert.DeserializeObject(jsonObject.ToString(), targetType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // Implement if needed
        throw new NotImplementedException();
    }
}
}
