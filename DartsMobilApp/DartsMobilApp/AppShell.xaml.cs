using CommunityToolkit.Maui.Views;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Services;

namespace DartsMobilApp
{
    public partial class AppShell : Shell
    {
        private readonly SignalRService _signalRService;
        public AppShell(SignalRService signalR)
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            LogOutPopUp popUp = new LogOutPopUp();
            Application.Current.MainPage.ShowPopup(popUp);
            bool confirm = await Shell.Current.DisplayAlert("Kijelentkezés", "Biztosan kijelentkezel?", "Igen", "Mégse");
            if (confirm)
            {
                SecureStorage.Default.SetAsync("SaveCheckedBool", "0");
               if (SecStoreItems.IsChecked == "1")
                {
                    LogOut.LogOutFunction();
                    SecureStorage.Remove("Token");
                    SecureStorage.Remove("RefreshToken");
                    SecureStorage.Remove("UserName");
                }
                else
                {
                    LogOut.LogOutFunction();
                    SecureStorage.RemoveAll();
                }

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            await _signalRService.DisconnectAsync();
        }
    }
}
