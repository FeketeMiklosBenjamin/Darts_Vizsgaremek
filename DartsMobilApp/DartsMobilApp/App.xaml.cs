using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Service;
using DartsMobilApp.Services;
using DartsMobilApp.ViewModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DartsMobilApp
{
    public partial class App : Application
    {
        private readonly SignalRService _signalR;
        public App(SignalRService service)
        {
            InitializeComponent();

            MainPage = new AppShell(service);
            _signalR = service;
        }

        protected override async void OnStart()
        {
            string? IsChecked = await SecureStorage.Default.GetAsync("SaveCheckedBool");
            if (IsChecked != null && IsChecked == "1")
            {
                var email = await SecureStorage.Default.GetAsync("Email");
                var password = await SecureStorage.Default.GetAsync("Password");
                LoginResponse response = await AuthService.LoginAsync(email, password);

                if (response.message == "Sikeres bejelentkezés.")
                {
                    await _signalR.ConnectAsync(response.accessToken);
                    await InitStorage();

                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
            }
        }
        private async Task InitStorage()
        {
            await SecStoreItems.InitAsync();
        }

    }
}
