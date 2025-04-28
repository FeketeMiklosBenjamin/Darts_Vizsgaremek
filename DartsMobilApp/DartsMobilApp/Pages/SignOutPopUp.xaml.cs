using CommunityToolkit.Maui.Views;
using DartsMobilApp.Classes;
using DartsMobilApp.Services;

namespace DartsMobilApp.Pages;

public partial class SignOutPopUp : Popup
{
    private readonly SignalRService _signalRService;
    public SignOutPopUp(SignalRService service)
    {
        InitializeComponent();
        _signalRService = service;
    }

    private async void LoggingOut(object sender, EventArgs e)
    {
        await SecureStorage.Default.SetAsync("SaveCheckedBool", "0");
        await LogOut.LogOutFunction();

        await _signalRService.DisconnectAsync();

        Shell.Current.FlyoutIsPresented = false;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        });

        this.Close();
    }

    private void NotLoggedOut(object sender, EventArgs e)
    {
        this.Close();
    }
}