using DartsMobilApp.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    public static class LogOut
    {
        public static async Task LogOutFunction()
        {
            var refreshTokenG = await SecureStorage.Default.GetAsync("RefreshToken");
            var RefreshToken = new RefreshTokenModel()
            {
                refreshToken = await SecureStorage.Default.GetAsync("RefreshToken")
            };
            var JsonContent = JsonSerializer.Serialize(RefreshToken);
            var content = new StringContent(JsonContent, Encoding.UTF8, "application/json");

            var response = await DartsAPI.PostLogout(content);
        }
    }
}
