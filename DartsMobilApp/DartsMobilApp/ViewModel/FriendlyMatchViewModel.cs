using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class FriendlyMatchViewModel : ObservableObject
    {

        private readonly SignalRService _signalRService;

        private readonly string UserId = SecStoreItems.UserId;
        private readonly string UserName = SecStoreItems.UserName;
        private readonly string Dartspoint = SecStoreItems.DartsPoints; 

        public FriendlyMatchViewModel()
        {

           
           _signalRService = new SignalRService();


            //_signalRService.OnFriendlyPlayerJoined += (joinedUserId, joinedUserName, joinedDartspoint) =>
            // {
            //     MainThread.BeginInvokeOnMainThread(() =>
            //     {
            //         JoinedPlayers.Add(new FriendlyPlayerModel
            //         {
            //             UserId = joinedUserId,
            //             Name = joinedUserName,
            //             DartrsPoint = joinedDartspoint
            //         });
            //         Shell.Current.DisplayAlert("Új játékos!", $"{joinedUserName} csatlakozott {joinedDartspoint}", "OK");
            //     });
            // };
        }

        [ObservableProperty]
        public ObservableCollection<FriendlyPlayerModel> joinedPlayers = new();


        [ObservableProperty]
        public List<FriendlyMatchModel>? friendlymatches;        

        [ObservableProperty]
        public List<FriendlyMatchModel> sortedFriendlies = new List<FriendlyMatchModel>();

        [ObservableProperty]
        private bool justPrivate = false;
        public List<FriendlyMatchModel> TakedFriendlies { get; set; } = new List<FriendlyMatchModel>();

        public string? AccessToken = SecStoreItems.AToken;

        [RelayCommand]
        private void Appearing()
        {
            _signalRService.ConnectAsync(SecStoreItems.AToken);
            LoadFriendliesMatch();
            SortedFriendlies = friendlymatches.Take(4).ToList();

        }

       


        [RelayCommand]
        private void JustPrivateMatches()
        {
            if (JustPrivate == true)
            {
                TakedFriendlies.Clear();
                Friendlymatches = Friendlymatches.Where(m => m.joinPassword != null && m.name != SecStoreItems.UserName && m.playerLevel == SecStoreItems.MyLevel).ToList();
                SortedFriendlies = Friendlymatches.Take(4).ToList();
                FillTakedTList();
            }
            else
            {
                TakedFriendlies.Clear();
                Friendlymatches = LoadFriendliesMatch();
                SortedFriendlies = Friendlymatches.Take(4).ToList();
                FillTakedTList();
            }
        }
           

       
        private List<FriendlyMatchModel> LoadFriendliesMatch()
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                throw new Exception("Hiányzó Access Token! Kérem jelentkezzen be!");

            }
            Friendlymatches = DartsAPI.GetFriendlyMatches().Where(match => match.name != SecStoreItems.UserName && match.playerLevel == SecStoreItems.MyLevel).ToList();
            
            if (Friendlymatches != null)
            {
                return Friendlymatches;
            }

            else
            {
                throw new Exception($"Valami hiba történt!");
            }
        }


        private List<FriendlyMatchModel> DeleteTakedItem()
        {
            foreach (var com in SortedFriendlies)
            {
                TakedFriendlies.Remove(com);
            }
            return TakedFriendlies;
        }

        private List<FriendlyMatchModel> FillTakedTList()
        {
            foreach (var co in SortedFriendlies)
            {
                TakedFriendlies.Add(co);
            }
            return TakedFriendlies;
        }

        [RelayCommand]
        private void FilterTournaments(string number)
        {
           
            if (number == "1")
            {
                if (Friendlymatches?.Count > 4 && TakedFriendlies.Count != Friendlymatches.Count)
                {
                    SortedFriendlies.Clear();
                    SortedFriendlies = Friendlymatches.Take(new Range(TakedFriendlies.Count, TakedFriendlies.Count + 4)).ToList();
                    FillTakedTList();
                }
            }
            else
            {
                DeleteTakedItem();
                if (TakedFriendlies.Count != 0)
                {
                    
                    SortedFriendlies.Clear();

                    SortedFriendlies = TakedFriendlies.Take(new Range(TakedFriendlies.Count - 4, TakedFriendlies.Count)).ToList();

                }
                else
                {
                    SortedFriendlies = Friendlymatches.Take(4).ToList();
                    FillTakedTList();
                }
            }
        }

        
        private  List<FriendlyMatchModel> RefreshFriendliesFunction()
        {
            List<FriendlyMatchModel> newFriendliesList = LoadFriendliesMatch().ToList();
            
            return newFriendliesList;
        }

        private List<FriendlyMatchModel> RefreshSorted()
        {
            Friendlymatches = RefreshFriendliesFunction();
            List<FriendlyMatchModel> newSortedFriendlies = Friendlymatches.Take(4).ToList();
            return newSortedFriendlies;
        }

        [RelayCommand]

        private void RefreshFriendlies()
        { 
            SortedFriendlies = RefreshSorted();
            TakedFriendlies = SortedFriendlies;
        }

        [RelayCommand]
        private async Task StartFriendlyMatch(string matchId)
        {
            WaitingForPlayersPopUp WaitingPopUp = new WaitingForPlayersPopUp();
            JoinRequestPopUpViewModel joinVm = new JoinRequestPopUpViewModel(_signalRService, matchId, SecStoreItems.UserId, SecStoreItems.UserName, SecStoreItems.DartsPoints);
            JoinRequestPopUp popUp = new JoinRequestPopUp(joinVm, matchId);
            _signalRService.JoinFriendlyMatch(matchId, SecStoreItems.UserId, SecStoreItems.UserName, SecStoreItems.DartsPoints);
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.ShowPopupAsync(WaitingPopUp);
            });
            
        }
    }
}
