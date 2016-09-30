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
        /// <summary>
        /// Serializes an object and encodes in UTF8 as JSON
        /// </summary>
        /// <param name="o">The object to serialize and encode</param>
        /// <param name="settings">Settings with which to serialize</param>
        /// <returns>Serialized object encoded in UTF as JSON</returns>
        public static HttpContent BuildHttpContent(object o, JsonSerializerSettings settings)
        {
            string postBody = JsonConvert.SerializeObject(o, settings);
            return new StringContent(postBody, Encoding.UTF8, "application/json");
        }
    }
}