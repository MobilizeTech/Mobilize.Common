using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mobilize.Common.Http
{
    /// Serializes JSON with camel case keys
    public static class CamelCaseJsonSerializer
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented, Settings);
        }

        public static T DeserializeObject<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s, Settings);
        }
    }
}