using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mobilize.Common.Http
{
    public static class RestClient
    {
        public static async Task<T> GetJsonData<T>(string url, string credentials = null)
        {
            var json = await GetJson<T>(url, credentials);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<IList<T>> GetJsonArray<T>(string url, string credentials = null)
        {
            var json = await GetJson<T>(url, credentials);
            var array = JArray.Parse(json);
            IList<T> objectsList = new List<T>();
            List<string> invalidJsonElements = null;

            foreach (var item in array)
            {
                try
                {
                    // CorrectElements
                    objectsList.Add(item.ToObject<T>());
                }
                catch (Exception)
                {
                    invalidJsonElements = invalidJsonElements ?? new List<string>();
                    invalidJsonElements.Add(item?.ToString());
                }
            }

            return objectsList;
        }

        private static async Task<string> GetJson<T>(string url, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(credentials))
                {
                    AddBasicAuthorizationHeader(credentials);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task<T> PostJsonData<T>(string url, HttpContent httpContent, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(credentials))
                {
                    AddBasicAuthorizationHeader(credentials);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(url, httpContent);
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
        }

        private static AuthenticationHeaderValue AddBasicAuthorizationHeader(string credentials)
        {
            var credentialsBytes = Encoding.UTF8.GetBytes(credentials);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentialsBytes));
        }
    }
}