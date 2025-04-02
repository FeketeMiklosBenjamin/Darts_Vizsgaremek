using DartsMobilApp.API;
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
        public AuthService()
        {
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
            



            try
            {
                var response = DartsAPI.PostLogin(content);

                if (response != null && response.role != 2)
                {

                    SecureStorage.SetAsync("Token", response.accessToken );
                    SecureStorage.SetAsync("Email", response.emailAddress );
                    SecureStorage.SetAsync("UserName", response.username);
                    SecureStorage.SetAsync("UserId", response.id);
                    SecureStorage.SetAsync("RefreshToken", response.refreshToken);
                    SecureStorage.SetAsync("Password", loginModel.Password);
                    return response;
                }
                else
                {
                    return new LoginResponse { message = "Hiba a bejelentkezés során!" };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse {  message = $"An error occurred: {ex.Message} " };
            }
        }
    }
}
