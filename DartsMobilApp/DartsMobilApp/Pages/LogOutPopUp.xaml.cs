using CommunityToolkit.Maui.Views;
using DartsMobilApp.Classes;
using DartsMobilApp.Services;
using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class LogOutPopUp : Popup
{
    private readonly SignalRService _signalRService;
	public LogOutPopUp(SignalRService service)
	{
		InitializeComponent();
        _signalRService = service;
    }
    private async void LoggingOut(object sender, EventArgs e)
    {
        await LogOut.LogOutFunction();

        await _signalRService.DisconnectAsync();

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }

    private void NotLoggedOut(object sender, EventArgs e)
    {
        this.Close();

    }
}