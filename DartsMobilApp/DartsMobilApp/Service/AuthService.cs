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
                var response = await DartsAPI.PostLogin(content);

                if (response != null && response.role != 2)
                {

                    await SecureStorage.Default.SetAsync("Token", response.accessToken );
                    await SecureStorage.Default.SetAsync("Email", response.emailAddress );
                    await SecureStorage.Default.SetAsync("UserName", response.username);
                    await SecureStorage.Default.SetAsync("UserId", response.id);
                    await SecureStorage.Default.SetAsync("DartsPoints", response.dartsPoints.ToString());
                    await SecureStorage.Default.SetAsync("MyLevel", response.level);
                    await SecureStorage.Default.SetAsync("RefreshToken", response.refreshToken);
                    await SecureStorage.Default.SetAsync("Password", loginModel.Password);
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
