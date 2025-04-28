using CommunityToolkit.Maui.Views;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;

using DartsMobilApp.Services;
using DartsMobilApp.ViewModels;

namespace DartsMobilApp
{
    public partial class AppShell : Shell
    {
        private readonly SignalRService _signalRService;
        public AppShell(SignalRService signalR)
        {
            InitializeComponent();
            _signalRService = signalR;
            BindingContext = new ShellViewModel();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            LogOutPopUp popUp = new LogOutPopUp(_signalRService);
            Application.Current.MainPage.ShowPopup(popUp);
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            SignOutPopUp popUp = new SignOutPopUp(_signalRService);
            Application.Current.MainPage.ShowPopup(popUp);
        }
    }
}