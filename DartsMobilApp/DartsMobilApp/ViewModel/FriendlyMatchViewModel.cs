using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class FriendlyMatchViewModel : ObservableObject
    {
        [ObservableProperty]
        public List<FriendlyMatchModel>? friendlymatches;


        [ObservableProperty]
        public List<FriendlyMatchModel> sortedFriendlies = new List<FriendlyMatchModel>();

        public List<FriendlyMatchModel> TakedFriendlies { get; set; } = new List<FriendlyMatchModel>();

        public string? AccessToken = SecStoreItems.AToken;

        [RelayCommand]
        private void Appearing()
        {
            
            LoadFriendliesMatch();
            SortedFriendlies = friendlymatches.Take(4).ToList();

        }

       
        private List<FriendlyMatchModel> LoadFriendliesMatch()
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                throw new Exception("Hiányzó Access Token! Kérem jelentkezzen be!");

            }
            Friendlymatches = DartsAPI.GetFriendlyMatches().Where(match => match.name != SecStoreItems.UserName).ToList();
            
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
        private async Task StartFriendlyMatch()
        {
            JoinRequestPopUp popUp = new JoinRequestPopUp();
            Application.Current.MainPage.ShowPopup(popUp);
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");
            });
            
        }
    }
}
