using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using DartsMobilApp.Services;
using DartsMobilApp.ViewModel;
using System.Text.RegularExpressions;

namespace DartsMobilApp.Pages;

public partial class WaitingForPlayersPopUp : Popup
{
	private readonly SignalRService signalR;
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
            if (popup is JoinRequestPopUp || popup is PasswordValidationPopUp)
            {
                popup.Close();
            }
            else
            {
                this.Close();
            }

            CounterViewModel.IsFriendlyMatch = true;
            CounterViewModel.settings = startingSetup;
            CounterViewModel.MatchId = matchId;
            await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");

            });
        };

        signalR.OnTournamentMatchStarted += async (startingSetup) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                this.Close();
                CounterViewModel.IsFriendlyMatch = false;
                CounterViewModel.settings = startingSetup;
                CounterViewModel.MatchId = matchId;
                await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");
            });
        };

    }
}