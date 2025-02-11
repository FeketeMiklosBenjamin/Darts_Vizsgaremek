using DartsMobilApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DartsMobilApp.Service
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        
        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var loginModel = new LoginModel
            {
                EmailAddress = email,
                Password = password
            };

            var jsonContent = JsonSerializer.Serialize(loginModel);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var apiUrl = "http://10.0.2.2:5181/api/users/login";

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseString);

                    return loginResponse;
                }
                else
                {
                    return new LoginResponse { IsSuccess = false, Message = "Login failed" };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse { IsSuccess = false, Message = $"An error occurred: {ex.Message} " };
            }
        }
    }
}
