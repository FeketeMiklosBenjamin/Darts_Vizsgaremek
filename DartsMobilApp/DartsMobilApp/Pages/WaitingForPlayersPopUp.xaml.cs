using CommunityToolkit.Maui.Views;
using DartsMobilApp.Services;

namespace DartsMobilApp.Pages;

public partial class WaitingForPlayersPopUp : Popup
{
	private SignalRService signalR;
	public WaitingForPlayersPopUp(SignalRService signalRService)
	{
		InitializeComponent();
        signalR = signalRService;
        signalR.OnFriendlyMatchStarted += async () =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");
            });
        };
    }
}