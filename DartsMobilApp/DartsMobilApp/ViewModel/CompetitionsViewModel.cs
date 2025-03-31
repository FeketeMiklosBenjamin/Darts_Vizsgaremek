using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{

    public partial class CompetitionsViewModel : ObservableObject
    {


        [ObservableProperty]
        public List<MatchModel>? tournaments;

        public string? AccessToken = SecStoreItems.AToken;

        [ObservableProperty]
        public List<MatchModel>? sortedTournaments;

        public List<MatchModel> TakedTournaments { get; set; } = new List<MatchModel>();


        [RelayCommand]

        private void Appearing()
        {
            LoadUserMatches();
            SortedTournaments = tournaments.Take(4).ToList();
        }

        private List<MatchModel> LoadUserMatches()
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                throw new Exception("Hiányzó Access Token! Kérem jelentkezzen be!");

            }
            Tournaments = DartsAPI.GetUserMatches();
            if (Tournaments != null)
            {
                return Tournaments;
            }

            else
            {
                throw new Exception($"Valami hiba történt!");
            }
        }


        private List<MatchModel> DeleteTakedItem()
        {
            foreach (var com in SortedTournaments)
            {
                TakedTournaments.Remove(com);
            }
            return TakedTournaments;
        }
        private List<MatchModel> FillTakedTList()
        {
            foreach (var co in sortedTournaments)
            {
                TakedTournaments.Add(co);
            }
            return TakedTournaments;
        }
        [RelayCommand]
        private void FilterTournaments(string number)
        {
            if (number == "1")
            {
                if (Tournaments?.Count > 4 && TakedTournaments.Count != Tournaments.Count)
                {
                    SortedTournaments.Clear();
                    SortedTournaments = Tournaments.Take(new Range(TakedTournaments.Count, TakedTournaments.Count +4)).ToList();
                    FillTakedTList();
                }
            }
            else
            {
                DeleteTakedItem();
                if (TakedTournaments.Count != 0)
                {
                    SortedTournaments.Clear();
                    
                    SortedTournaments = TakedTournaments.Take(new Range(TakedTournaments.Count - 4, TakedTournaments.Count)).ToList();
                   
                }
                else
                {
                    SortedTournaments = Tournaments.Take(4).ToList();
                    FillTakedTList();
                }
            }
        }

        [RelayCommand]
        private async Task StartMatch()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(CounterPage)}");
            });
        }

    }
}
