using CommunityToolkit.Maui.Views;
using DartsMobilApp.Classes;
using DartsMobilApp.SecureStorageItems;
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
        await _signalRService.DisconnectAsync();
    }

    private void NotLoggedOut(object sender, EventArgs e)
    {
        this.Close();

    }
}