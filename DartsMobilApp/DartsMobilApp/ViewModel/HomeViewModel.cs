using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {

        [RelayCommand]

        private async void GoToFriendlySetup()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(FriendlySetupPage)}", true);
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
