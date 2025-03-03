using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.Classes;
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
        public List<TournamentModel> sortedTournaments;

        [ObservableProperty]
        public ObservableCollection<TournamentModel> tournaments = new ObservableCollection<TournamentModel>
        {
            new TournamentModel { Name = "Hungarian Darts Trophy", Level = "Profi", Time = DateTime.Now },
            new TournamentModel { Name = "UK Open", Level = "Amatőr", Time = DateTime.Now },
            new TournamentModel{Name = "PDC Darts Világbajnokság",Level = "Profi",Time = new DateTime(2025, 12, 1) },
            new TournamentModel{Name = "Premier League Darts",Level = "Profi",Time =  new DateTime(2025, 2, 1)},
            new TournamentModel{Name= "World Grand Prix",Level= "Profi",Time= new DateTime(2025, 10, 1)},
            new TournamentModel{Name ="BDO Darts Világbajnokság",Level = "Amatőr/Profi",Time= new DateTime(2025, 1, 1)},
            new TournamentModel{Name ="World Masters", Level = "Amatőr/Profi",Time= new DateTime(2025, 10, 1)},
            new TournamentModel{Name ="Nemzeti Bajnokság", Level = "Amatőr",Time = new DateTime(2025, 5, 1)},
            new TournamentModel{Name= "Kisalföld Darts Liga", Level = "Kezdő/Amatőr", Time = new DateTime(2025, 6, 1)},
        };


        //[RelayCommand]
        //private void FilterTournaments(int number)
        //{
        //    sortedTournaments = tournaments.Take(4).ToList();
        //    if (number == 1)
        //    {
        //        sortedTournaments.Clear();
        //        sortedTournaments = tournaments.Take(4).ToList();
                
        //    }
        //}

    }
}
