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

    }
}
