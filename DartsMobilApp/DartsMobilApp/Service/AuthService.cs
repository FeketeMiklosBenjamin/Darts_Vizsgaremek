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
    public static class AuthService
    {

        public static async Task<LoginResponse> LoginAsync(string email, string password)
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

                    await SecureStorage.SetAsync("Token", response.accessToken );
                    await SecureStorage.SetAsync("Email", response.emailAddress );
                    await SecureStorage.SetAsync("UserName", response.username);
                    await SecureStorage.SetAsync("UserId", response.id);
                    await SecureStorage.SetAsync("DartsPoints", response.dartsPoints.ToString());
                    await SecureStorage.SetAsync("MyLevel", response.level);
                    await SecureStorage.SetAsync("RefreshToken", response.refreshToken);
                    await SecureStorage.SetAsync("Password", loginModel.Password);
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
