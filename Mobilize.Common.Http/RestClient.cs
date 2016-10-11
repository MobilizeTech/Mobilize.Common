using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        public static T Get<T>(string url, string credentials = null)
        {
            var json = GetJson<T>(url, credentials);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Asynchronously retrive and deserialize JSON data
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>Deserialized object</returns>
        public static async Task<T> GetAsync<T>(string url, string credentials = null)
        {
            var json = await GetJsonAsync<T>(url, credentials);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Retrive JSON arrays without a root element
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>List of objects</returns>
        public static IList<T> GetArray<T>(string url, string credentials = null)
        {
            var json = GetJsonAsync<T>(url, credentials).Result;
            return JsonHelper.ParseJsonArray<T>(json);
        }

        /// <summary>
        /// Asynchronously retrieve JSON arrays without a root element
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>List of objects</returns>
        public static async Task<IList<T>> GetArrayAsync<T>(string url, string credentials = null)
        {
            var json = await GetJsonAsync<T>(url, credentials);
            return JsonHelper.ParseJsonArray<T>(json);           
        }
 
        /// <summary>
        /// Post JSON data to a URL
        /// </summary>
        /// <param name="url">URL to which to POST</param>
        /// <param name="httpContent">Content to POST</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>Deserialized object</returns>
        public static T Post<T>(string url, HttpContent content, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                AddRequestHeaders(client, credentials);
                try
                {
                    var result = client.PostAsync(url, content).Result;
                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        return default(T);
                    }

                    return JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Asynchronously post JSON data to a URL
        /// </summary>
        /// <param name="url">URL to which to POST</param>
        /// <param name="content">Content to POST</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>Deserialized object</returns>
        public static async Task<T> PostAsync<T>(string url, HttpContent content, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                AddRequestHeaders(client, credentials);
                var result = await client.PostAsync(url, content);
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Post JSON data to a URL with blank response
        /// </summary>
        /// <param name="url">URL to which to POST</param>
        /// <param name="content">Content to POST</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>True if successfully processed, otherwise false</returns>
        public static bool PostBlankResponse(string url, HttpContent content, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                AddRequestHeaders(client, credentials);
                try
                {
                    var httpResponseMessage = client.PostAsync(url, content).Result;
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent ||
                        httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }

                return false;
            }
        }
 
        /// <summary>
        /// Retrives JSON data
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>JSON string</returns>
        private static async Task<string> GetJsonAsync<T>(string url, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                AddRequestHeaders(client, credentials);
                var result = await client.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Retrives JSON data
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>JSON string</returns>
        private static string GetJson<T>(string url, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                AddRequestHeaders(client, credentials);
                var result = client.GetAsync(url).Result;
                return result.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// Adds request headers
        /// </summary>
        /// <param name="client">Client for which to add headers</param>
        /// <param name="credentials">Credentials to use for basic authentication header</param>
        private static void AddRequestHeaders(HttpClient client, string credentials = null)
        {
            AddBasicAuthorizationHeader(client, credentials);
            AddAcceptJsonHeader(client);
        }

        /// <summary>
        /// Adds a basic authentication header 
        /// </summary>
        /// <param name="client">Client for which to add headers</param>
        /// <param name="credentials">Credentials to use for basic authentication header</param>
        private static void AddBasicAuthorizationHeader(HttpClient client, string credentials)
        {
            if (!string.IsNullOrWhiteSpace(credentials))
            {
                var credentialsBytes = Encoding.UTF8.GetBytes(credentials);
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentialsBytes));
                client.DefaultRequestHeaders.Authorization = header;
            }
        }

        /// <summary>
        /// Adds a JSON accept header
        /// </summary>
        /// <param name="client">Client for which to add headers</param>
        private static void AddAcceptJsonHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}