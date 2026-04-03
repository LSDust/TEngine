using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TEngine
{
    public class NewtonsoftJsonHelper : Utility.Json.IJsonHelper
    {
        public static JsonSerializerSettings Settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Error
        };

        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public object ToObject(Type objectType, string json)
        {
            return JsonConvert.DeserializeObject(json, objectType, Settings);
        }
    }
}
