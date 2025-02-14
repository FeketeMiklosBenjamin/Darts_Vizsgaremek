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

        [RelayCommand]
        private void Apperaring()
        {
        }





        [RelayCommand]

        private async void GoToHomePage()
        {
            var email = EmailAddress;
            var password = Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both username and password", "OK");
                
            }

            var loginResponse = await _authService.LoginAsync(email, password);

            if (loginResponse.message == "Sikeres bejelentkezés.")
            {
                // Handle successful login (e.g., navigate to the main page)
                await Application.Current.MainPage.DisplayAlert("Success", "Login successful", "OK");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                });
            }
            else
            {
                // Handle failed login
                await Application.Current.MainPage.DisplayAlert("Error", loginResponse.message, "OK");
            }



        }


    }
}
