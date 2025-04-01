using DartsMobilApp.Classes;
using DartsMobilApp.SecureStorageItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DartsMobilApp.Network
{
    public class HTTPCommunication<T> where T : class
    {

        public static async Task<T?> Get(string url)
            {
            string? AToken = SecStoreItems.AToken;
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AToken); 
                using var response = await client.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string resultStr = await response.Content.ReadAsStringAsync();
                    T? result = JsonSerializer.Deserialize<T>(resultStr);
                    return result;
                }
                return null;
            }
        
        public static async Task<T> Post(string url, StringContent content)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            using var response = await client.SendAsync(request).ConfigureAwait(false);
            if(response.IsSuccessStatusCode)
            {
                string resultStr = await response.Content.ReadAsStringAsync();
                T result = JsonSerializer.Deserialize<T>(resultStr);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            return null;
            
        }
        public static async Task<T> PostAToken(string url, StringContent content)
        {
            string? AToken = SecStoreItems.AToken;
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            request.Content = content;
            using var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string resultStr = await response.Content.ReadAsStringAsync();
                T result = JsonSerializer.Deserialize<T>(resultStr);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            return null;

        }

        //public static T PostWithHeader<T>(string url, HttpContent content)
        //{
        //    string? AToken = SecStoreItems.AToken;
        //    using var client = new HttpClient();
        //    var request = new HttpRequestMessage(HttpMethod.Post, url);
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
        //    request.Content = content;
        //    var response = client.PostAsync(url, content).Result;

        //    // Ellenőrizzük a válasz státuszkódját
        //    if (response.StatusCode == HttpStatusCode.NoContent)
        //    {
        //        return default(T);
        //    }

        //    var responseContent = response.Content.ReadAsStringAsync().Result;

        //    // Ha a válasz törzse üres, ne próbáljuk deszerializálni
        //    if (string.IsNullOrWhiteSpace(responseContent))
        //    {
        //        return default(T);
        //    }

        //    // Deszerializálás JSON-re
        //    return JsonSerializer.Deserialize<T>(responseContent);

        //}
    }
}
