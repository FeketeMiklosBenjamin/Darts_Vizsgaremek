using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Pages;
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

        [ObservableProperty]
        private string matchId;

        [ObservableProperty]
        private string userId;
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private string darts_point;

        public JoinRequestPopUpViewModel(SignalRService signalRService,string MatchId, string playerId, string Username, string dartsPoint)
        {
            _signalRService = signalRService;
            matchId = MatchId;
            userId = playerId;
            userName = Username;
            darts_point = dartsPoint;
        }


        [RelayCommand]
        private async void AcceptJoinRequest()
        {
            await _signalRService.StartFriendlyMatch(matchId, SecStoreItems.UserId, UserId);
        }

    }
}
