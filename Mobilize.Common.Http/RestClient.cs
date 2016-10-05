using System;
using System.Net;
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
        /// <summary>
        /// Retrive and deserialize JSON data
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>Deserialized object</returns>
        public static async Task<T> GetJsonData<T>(string url, string credentials = null)
        {
            var json = await GetJson<T>(url, credentials);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Retrive JSON arrays without a root element
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>List of objects</returns>
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

        /// <summary>
        /// Retrives JSON data
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>JSON string</returns>
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

        /// <summary>
        /// Posts JSON data to a URL
        /// </summary>
        /// <param name="url">URL to which to POST</param>
        /// <param name="httpContent">Content to POST</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>Deserialized object</returns>
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

        /// <summary>
        /// Posts no content to a URL
        /// </summary>
        /// <param name="url">URL to which to POST</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>True if successfully processed, otherwise false</returns>
        public static bool PostNoContent(string url, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(credentials))
                {
                    AddBasicAuthorizationHeader(credentials);
                }

                try
                {
                    var httpResponseMessage = client.PostAsync(url, null).Result;

                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return true;
                    }
                }
                catch (OperationCanceledException)
                {
                    return false;
                }

                return false;
            }
        }

        /// <summary>
        /// Creates a basic authentication header 
        /// </summary>
        /// <param name="credentials">Credentials to use for basic authentication header</param>
        /// <returns>Base64 encoded Authentication header</returns>
        private static AuthenticationHeaderValue AddBasicAuthorizationHeader(string credentials)
        {
            var credentialsBytes = Encoding.UTF8.GetBytes(credentials);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentialsBytes));
        }
    }
}