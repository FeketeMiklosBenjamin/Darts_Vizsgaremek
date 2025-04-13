using DartsMobilApp.API;
using DartsMobilApp.SecureStorageItems;
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
        public static void LogOutFunction()
        {
            var RefreshToken = new RefreshTokenModel()
            {
                refreshToken = SecStoreItems.RToken
            };
            var JsonContent = JsonSerializer.Serialize(RefreshToken);
            var content = new StringContent(JsonContent, Encoding.UTF8, "application/json");

            var response = DartsAPI.PostLogout(content);
        }
    }
}
