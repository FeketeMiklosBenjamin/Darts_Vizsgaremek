using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
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
            TakedFriendlies = FillTakedTList();
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

        
        private  List<FriendlyMatchModel> RefreshFriendfliesFunction()
        {
            Friendlymatches = DartsAPI.GetFriendlyMatches();
            
            return Friendlymatches;
        }

        private List<FriendlyMatchModel> RefreshSorted()
        {
            SortedFriendlies = RefreshFriendfliesFunction().Take(4).ToList();
            return SortedFriendlies;
        }


        [RelayCommand]

        private void RefreshFriendlies()
        {
            RefreshFriendfliesFunction();
            RefreshSorted();
        }
    }
}
