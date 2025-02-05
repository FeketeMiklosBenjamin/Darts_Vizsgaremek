using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.Database;
using DartsMobilApp.Pages;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {

        [ObservableProperty]
        public List<UserLoginData> userDatas;
        
        
        [RelayCommand]
        private  void Apperaring()
        {
            userDatas = Task.Run(() => LoginDatabase.GetAllItemsAsync()).Result;
        }

        [RelayCommand]

        private async void GoToHomePage()
        {
            MainThread.BeginInvokeOnMainThread(async () => 
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
            });
        }

            
    }
}
