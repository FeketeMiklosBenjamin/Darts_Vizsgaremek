using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Service;
using DartsMobilApp.Services;
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

        private readonly SignalRService _signalR;

        public LoginViewModel(SignalRService signalR)
        {
            _signalR = signalR;
        }
        //[ObservableProperty]
        //public List<L> userDatas;
        [ObservableProperty]
        public string emailAddress;

        [ObservableProperty]
        public string password;


        [ObservableProperty]
        public bool saveChecked;

        AutomaticLogInPopUp autoLogInP = new AutomaticLogInPopUp();

        private string isChecked;

        [RelayCommand]
        private void Appearing()
        {
            isChecked = SecStoreItems.IsChecked;
            if (isChecked != null && isChecked == "1")
            {
                Application.Current.MainPage.ShowPopup(autoLogInP);
            }
        }


        private static TimerCountDown timer;

        [RelayCommand]
        private void Disappearing()
        {
   
            if (isChecked != null && isChecked == "1")
            {
                autoLogInP.Close();
            }
            
        }



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
           
            
            var loginResponse = await AuthService.LoginAsync(email, password);

            if (loginResponse.message == "Sikeres bejelentkezés.")
            {
                LoginPopUp loginPopUp = new LoginPopUp();
                Application.Current.MainPage.ShowPopupAsync(loginPopUp);

                if (timer == null)
                {
                    timer = new TimerCountDown();
                    timer.Start();
                }

                await _signalR.ConnectAsync(SecureStorage.GetAsync("Token").Result);
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Hiba!", loginResponse.message, "OK");
            }
        }

    }
}