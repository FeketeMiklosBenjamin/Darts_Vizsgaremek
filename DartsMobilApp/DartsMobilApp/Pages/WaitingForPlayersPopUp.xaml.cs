using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using DartsMobilApp.Services;
using DartsMobilApp.ViewModel;
using DartsMobilApp.Pages;
using System.Threading.Tasks;
using DartsMobilApp.Classes;

namespace DartsMobilApp.Pages
{
    public partial class WaitingForPlayersPopUp : Popup
    {
        private readonly SignalRService signalR;
        private Popup popup;
        private string _matchId;

        public WaitingForPlayersPopUp(SignalRService signalRService, string matchId)
        {
            InitializeComponent();
            signalR = signalRService;
            _matchId = matchId;
            signalR.OnFriendlyPlayerJoined += OnFriendlyPlayerJoined;
            signalR.OnFriendlyPlayerRemoved += OnFriendlyPlayerRemoved;
            signalR.OnFriendlyMatchStarted += OnFriendlyMatchStarted;
            signalR.OnTournamentMatchStarted += OnTournamentMatchStarted;
        }

        private async void OnFriendlyPlayerJoined(string playerId, string username, string dartsPoint)
        {
            var popupPage = new JoinRequestPopUp(signalR, _matchId, playerId, username, dartsPoint);
            popup = popupPage;
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.ShowPopupAsync(popupPage);
            });
        }

        private async void OnFriendlyPlayerRemoved()
        {
            ClosePopup();

            LoginPopUp lg = new LoginPopUp();
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.ShowPopupAsync(lg);
            });
            this.Close();
        }

        private void OnFriendlyMatchStarted(StartFriendlyMatchModel startingSetup)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                ClosePopup();
                this.Close();

                CounterViewModel.IsFriendlyMatch = true;
                CounterViewModel.settings = startingSetup;
                CounterViewModel.MatchId = _matchId;
                await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");
            });
        }

        private async void OnTournamentMatchStarted(StartFriendlyMatchModel startingSetup)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                ClosePopup();
                this.Close();

                CounterViewModel.IsFriendlyMatch = false;
                CounterViewModel.settings = startingSetup;
                CounterViewModel.MatchId = _matchId;
                await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");
            });
        }

        public void ClosePopup()
        {
            signalR.OnFriendlyPlayerJoined -= OnFriendlyPlayerJoined;
            signalR.OnFriendlyPlayerRemoved -= OnFriendlyPlayerRemoved;
            signalR.OnFriendlyMatchStarted -= OnFriendlyMatchStarted;
            signalR.OnTournamentMatchStarted -= OnTournamentMatchStarted;
        }
    }
}