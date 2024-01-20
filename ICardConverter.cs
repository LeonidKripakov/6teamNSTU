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

            // Извлекаем свойство $type, чтобы определить фактический тип
            string typeName = (string)jsonObject["$type"];
            Type targetType = Type.GetType(typeName);

            // Десериализуем в конкретный тип
            return JsonConvert.DeserializeObject(jsonObject.ToString(), targetType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
