using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.SecureStorageItems;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DartsMobilApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }


        //private void LogOut()
        //{
        //    var RefreshToken = new RefreshTokenModel()
        //    {
        //        refreshToken = SecStoreItems.RToken
        //    };
        //    var JsonContent = JsonSerializer.Serialize(RefreshToken);
        //    var content = new StringContent(JsonContent, Encoding.UTF8, "application/json");

        //    var response = DartsAPI.PostLogout(content);

        //    Debug.WriteLine($"\n\n\n\nResponse: {response}\n\n\n\n");
        //}

    }
}
