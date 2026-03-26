using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TEngine
{
    public class NewtonsoftJsonHelper : Utility.Json.IJsonHelper
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Error
        };

        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public object ToObject(Type objectType, string json)
        {
            return JsonConvert.DeserializeObject(json, objectType, _settings);
        }
    }
}
