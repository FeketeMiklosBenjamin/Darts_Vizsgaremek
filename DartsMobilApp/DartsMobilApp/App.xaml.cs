using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Service;
using DartsMobilApp.Services;
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
                LoginResponse response = await AuthService.LoginAsync(SecStoreItems.Email, SecStoreItems.Password);

                if (response.message == "Sikeres bejelentkezés.")
                {
                    await _signalR.ConnectAsync(SecStoreItems.AToken);
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                    SecureStorage.SetAsync("Token", response.accessToken);
                    SecureStorage.SetAsync("Email", response.emailAddress);
                    SecureStorage.SetAsync("UserName", response.username);
                    SecureStorage.SetAsync("UserId", response.id);
                    SecureStorage.SetAsync("RefreshToken", response.refreshToken);
                    SecureStorage.SetAsync("Password", SecStoreItems.Password);
                    SecureStorage.SetAsync("Email", SecStoreItems.Email);
                    
                }
            }
            

        }
    }
}
