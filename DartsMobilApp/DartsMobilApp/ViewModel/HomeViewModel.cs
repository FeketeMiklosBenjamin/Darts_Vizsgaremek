using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {
        }
        [RelayCommand]

        private async void GoToFriendlies()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(FriendlyMatchPage)}", true);
            });
        }

        [RelayCommand]

        private async void GoToCompetitionPage()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(CompetitionsPage)}", true);
            });
        }


    }
}
