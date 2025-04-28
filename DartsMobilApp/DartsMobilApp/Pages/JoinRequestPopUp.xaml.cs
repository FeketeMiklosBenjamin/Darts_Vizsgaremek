using CommunityToolkit.Maui.Views;
using DartsMobilApp.Services;
using DartsMobilApp.SecureStorageItems;
using System;
using System.Threading.Tasks;

namespace DartsMobilApp.Pages
{
    public partial class JoinRequestPopUp : Popup
    {
        private readonly SignalRService _signalRService;
        private readonly string _matchId;
        private readonly string _userId;
        private readonly string _userName;
        private readonly string _dartsPoint;

        public JoinRequestPopUp(SignalRService signalRService, string matchId, string playerId, string username, string dartsPoint)
        {
            InitializeComponent();
            _signalRService = signalRService;
            _matchId = matchId;
            _userId = playerId;
            _userName = username;
            _dartsPoint = dartsPoint;

            UserNameLabel.Text = _userName;
            DartsPointLabel.Text = _dartsPoint;
        }

        private async void AcceptJoinRequest(object sender, EventArgs e)
        {
            await _signalRService.StartFriendlyMatch(_matchId, SecStoreItems.UserId, _userId);
            this.Close();
        }

        private async void RejectJoinRequest(object sender, EventArgs e)
        {
            await _signalRService.RemovePlayerFromFriendlyMatch(_matchId, SecStoreItems.UserId, _userId);
            this.Close();
        }
    }
}
