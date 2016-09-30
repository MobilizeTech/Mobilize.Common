using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mobilize.Common.Http
{
    public static class JsonHelper
    {
        public static HttpContent BuildHttpContent(object o, JsonSerializerSettings settings)
        {
            string postBody = JsonConvert.SerializeObject(o, settings);
            return new StringContent(postBody, Encoding.UTF8, "application/json");
        }
    }
}