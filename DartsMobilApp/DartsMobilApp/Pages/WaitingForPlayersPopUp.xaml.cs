using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using DartsMobilApp.Services;
using DartsMobilApp.ViewModel;
using System.Text.RegularExpressions;

namespace DartsMobilApp.Pages;

public partial class WaitingForPlayersPopUp : Popup
{
	private SignalRService signalR;
    private Popup popup= new Popup();
	public WaitingForPlayersPopUp(SignalRService signalRService, string matchId)
	{

		InitializeComponent();
        signalR = signalRService;
        signalR.OnFriendlyPlayerJoined += async (playerId, username, dartsPoint) =>
        {
            var popupVm = new JoinRequestPopUpViewModel(signalR, matchId, playerId, username, dartsPoint);
            var popupPage = new JoinRequestPopUp(popupVm, matchId);
            popup = popupPage;
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                this.Close();
                await Application.Current.MainPage.ShowPopupAsync(popupPage);
            });
        };
        signalR.OnFriendlyMatchStarted += async (startingSetup) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (popup is JoinRequestPopUp)
                {
                    popup.Close();
                }
                else
                {
                    this.Close();
                }
                await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");

            });
        };
    }
}