using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class PasswordValidationPopUpViewModel : ObservableObject
    {
        private readonly SignalRService _signalRService;

        public static string MatchId;

        [ObservableProperty]

        private string password;

        public PasswordValidationPopUpViewModel(SignalRService service)
        {
                _signalRService = service;
        }

        [RelayCommand]

        private  void CheckValidPassword(string password)
        {
            ValidatePassword ValidatePassword = new ValidatePassword()
            {
                password = password
            };
            var jsonContent = JsonSerializer.Serialize(ValidatePassword);
            var response = DartsAPI.ValidatePassword(new StringContent(jsonContent,Encoding.UTF8, "application/json"), MatchId);
            if (response.message == "Sikeres belépés!")
            {
                WaitingForPlayersPopUp waitingPopUp = new WaitingForPlayersPopUp(_signalRService, MatchId);
                _signalRService.JoinTournamentMatch(MatchId, SecStoreItems.UserId);
                
                 Application.Current.MainPage.ShowPopup(waitingPopUp);
            }
        }
    }
}
