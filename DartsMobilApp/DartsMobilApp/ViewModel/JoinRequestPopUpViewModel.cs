using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{

    public partial class JoinRequestPopUpViewModel : ObservableObject
    {
        private readonly SignalRService _signalRService;

        private string matchId;

        private string userId;

        private string userName;

        private string darts_point;

        public JoinRequestPopUpViewModel(SignalRService signalRService,string MatchId, string playerId, string Username, string dartsPoint)
        {
            _signalRService = signalRService;
            _signalRService.ConnectAsync(SecStoreItems.AToken);
            matchId = MatchId;
            userId = playerId;
            userName = Username;
            darts_point = dartsPoint;
        }


        [RelayCommand]

        private void AcceptJoinRequest()
        {
        }
    }
}
