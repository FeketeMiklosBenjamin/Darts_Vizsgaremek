using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.SecureStorageItems
{
    public static class SecStoreItems
    {
        public static string? AToken { get; private set; }
        public static string? RToken { get; private set; }
        public static string? UserId { get; private set; }
        public static string? UserName { get; private set; }
        public static string? IsChecked { get; private set; }
        public static string? Password { get; private set; }
        public static string? Email { get; private set; }
        public static string? MyLevel { get; private set; }
        public static string? DartsPoints { get; private set; }

        public static async Task InitAsync()
        {
            AToken = await SecureStorage.Default.GetAsync("Token");
            RToken = await SecureStorage.Default.GetAsync("RefreshToken");
            UserId = await SecureStorage.Default.GetAsync("UserId");
            UserName = await SecureStorage.Default.GetAsync("UserName");
            IsChecked = await SecureStorage.Default.GetAsync("SaveCheckedBool");
            Password = await SecureStorage.Default.GetAsync("Password");
            Email = await SecureStorage.Default.GetAsync("Email");
            MyLevel = await SecureStorage.Default.GetAsync("MyLevel");
            DartsPoints = await SecureStorage.Default.GetAsync("DartsPoints");
        }
    }
}
