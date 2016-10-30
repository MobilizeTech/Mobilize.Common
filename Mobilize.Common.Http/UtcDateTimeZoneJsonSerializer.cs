using Newtonsoft.Json;

namespace Mobilize.Common.Http
{
    /// <summary>
    /// Deserializes JSON with UTC date/times
    /// </summary>
    public static class UtcDateTimeZoneJsonSerializer
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
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