using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mobilize.Common.Http
{
    public static class RestClient
    {
        /// <summary>
        /// Retrieve and deserialize JSON data
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <param name="settings">Serializer settings to use</param>
        /// <returns>Deserialized object</returns>
        public static T Get<T>(string url, string credentials = null, JsonSerializerSettings settings = null)
        {
            var json = GetJson<T>(url, credentials);
            return DeserializeJson<T>(json, settings);
        }

        /// <summary>
        /// Asynchronously retrieve and deserialize JSON data
        /// </summary>
        /// <param name="url">URL from where to retrieve</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <param name="settings">Serializer settings to use</param>
        /// <returns>Deserialized object</returns>
        public static async Task<T> GetAsync<T>(string url, string credentials = null, JsonSerializerSettings settings = null)
        {
            var json = await GetJsonAsync<T>(url, credentials);
            return DeserializeJson<T>(json, settings);
        }

        /// <summary>
        /// Retrieve JSON arrays without a root element
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
        /// <param name="content">Content to POST</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <param name="settings">Serializer settings to use</param>
        /// <returns>Deserialized object</returns>
        public static T Post<T>(string url, HttpContent content, string credentials = null, JsonSerializerSettings settings = null)
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

                    var json = result.Content.ReadAsStringAsync().Result;
                    return DeserializeJson<T>(json, settings);
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
        /// <param name="settings">Serializer settings to use</param>
        /// <returns>Deserialized object</returns>
        public static async Task<T> PostAsync<T>(string url, HttpContent content, string credentials = null, JsonSerializerSettings settings = null)
        {
            using (var client = new HttpClient())
            {
                AddRequestHeaders(client, credentials);
                var result = await client.PostAsync(url, content);
                var json = await result.Content.ReadAsStringAsync();
                return DeserializeJson<T>(json, settings);
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
                    var response = client.PostAsync(url, content).Result;
                    return IsBlankResponse(response);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Asynchronously post JSON data to a URL with blank response
        /// </summary>
        /// <param name="url">URL to which to POST</param>
        /// <param name="content">Content to POST</param>
        /// <param name="credentials">Credentials for basic authentication header</param>
        /// <returns>True if successfully processed, otherwise false</returns>
        public async static Task<bool> PostBlankResponseAsync(string url, HttpContent content, string credentials = null)
        {
            using (var client = new HttpClient())
            {
                AddRequestHeaders(client, credentials);
                try
                {
                    var response = await client.PostAsync(url, content);
                    return IsBlankResponse(response);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieve JSON data
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
        /// Retrieve JSON data
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
        /// Deserializes JSON data
        /// </summary>
        /// <typeparam name="T">Type to which to deserialize</typeparam>
        /// <param name="json">Json to deserialize</param>
        /// <param name="settings">Serializer settings to use</param>
        /// <returns>Deserialized object</returns>
        private static T DeserializeJson<T>(string json, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// Checks for successful blank response 
        /// </summary>
        /// <param name="response">Response message to check</param>
        /// <returns>True if successfully processed, otherwise false</returns>
        public static bool IsBlankResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NoContent ||
                response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add request headers
        /// </summary>
        /// <param name="client">Client for which to add headers</param>
        /// <param name="credentials">Credentials to use for basic authentication header</param>
        private static void AddRequestHeaders(HttpClient client, string credentials = null)
        {
            AddBasicAuthorizationHeader(client, credentials);
            AddAcceptJsonHeader(client);
        }

        /// <summary>
        /// Add a basic authentication header 
        /// </summary>
        /// <param name="client">Client for which to add headers</param>
        /// <param name="credentials">Credentials to use for basic authentication header</param>
        private static void AddBasicAuthorizationHeader(HttpClient client, string credentials)
        {
            if (!string.IsNullOrWhiteSpace(credentials))
            {
                var encodedCredentials = Base64Encode(credentials);
                var header = new AuthenticationHeaderValue("Basic", encodedCredentials);
                client.DefaultRequestHeaders.Authorization = header;
            }
        }

        /// <summary>
        /// Base 64 encode a string
        /// </summary>
        /// <param name="s">String to encode</param>
        /// <returns>Encoded string</returns>
        public static string Base64Encode(string s)
        {
            var credentialsBytes = Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(credentialsBytes);
        }

        /// <summary>
        /// Add a JSON accept header
        /// </summary>
        /// <param name="client">Client for which to add headers</param>
        private static void AddAcceptJsonHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}