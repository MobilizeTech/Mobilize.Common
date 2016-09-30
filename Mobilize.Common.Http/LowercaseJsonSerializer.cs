using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mobilize.Common.Http
{
    public static class LowercaseJsonSerializer
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new LowercaseContractResolver()
        };

        public static string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented, Settings);
        }

        public static T DeserializeObject<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s, Settings);
        }
 
        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }
    }
}