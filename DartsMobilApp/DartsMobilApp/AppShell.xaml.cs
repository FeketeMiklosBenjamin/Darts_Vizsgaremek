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
            _signalRService = signalR;
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            LogOutPopUp popUp = new LogOutPopUp(_signalRService);
            Application.Current.MainPage.ShowPopup(popUp);
        }
    }
}
