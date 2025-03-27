using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.SecureStorageItems
{
    public static class SecStoreItems
    {
        public static string? AToken { get; set; } = SecureStorage.GetAsync("Token")?.Result?.ToString();

        public static string? RToken { get; set; } = SecureStorage.GetAsync("RefreshToken")?.Result?.ToString();

        public static string? UserId { get; set; } = SecureStorage.GetAsync("UserId")?.Result?.ToString();

        public static string? UserName { get; set; } = SecureStorage.GetAsync("UserName")?.Result?.ToString();

    }
}
