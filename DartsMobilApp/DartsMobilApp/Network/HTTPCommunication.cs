using DartsMobilApp.SecureStorageItems;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
