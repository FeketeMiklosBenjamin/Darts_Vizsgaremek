using Android.Database;
using CommunityToolkit.Mvvm.ComponentModel;
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
        public ObservableCollection<TournamentModel> tournaments = new ObservableCollection<TournamentModel>
        {
            new TournamentModel { Name = "Hungarian Darts Trophy", Level = "Profi", Time = DateTime.Now },
            new TournamentModel { Name = "UK Open", Level = "Amatőr", Time = DateTime.Now }
        };

    }
}
