using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.Service;
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

        private  AuthService _authService = new AuthService();
        //[ObservableProperty]
        //public List<L> userDatas;
        [ObservableProperty]
        public string emailAddress;

        [ObservableProperty]
        public string password;


        [ObservableProperty]
        public bool saveChecked;

        [RelayCommand]
        private void Apperaring()
        {
            
        }


        private static TimerCountDown timer; 





        [RelayCommand]

        private async void GoToHomePage()
        {
            var email = EmailAddress;
            var password = Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await Application.Current.MainPage.DisplayAlert("Hiba!", "A bejelentkezés nem lehetséges! Az email és a jelszó mező kitöltése kötelező!", "OK");
                
            }

            if (SaveChecked)
                await SecureStorage.Default.SetAsync("SaveCheckedBool", "1");
            else
               await SecureStorage.Default.SetAsync("SaveCheckedBool", "0");
           
            
            var loginResponse = await _authService.LoginAsync(email, password);

            if (loginResponse.message == "Sikeres bejelentkezés.")
            {
                await Application.Current.MainPage.DisplayAlert("Sikeresen Bejelntkezett" , $"{loginResponse.message}", "OK");

                if (timer == null)
                {
                    timer = new TimerCountDown();
                    timer.Start();
                }

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Hiba!", loginResponse.message, "OK");
            }



        }


    }
}



// Timer Set Up
