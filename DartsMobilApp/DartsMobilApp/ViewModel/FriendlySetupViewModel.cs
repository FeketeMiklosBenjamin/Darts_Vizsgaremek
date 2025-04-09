using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Services;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class FriendlySetupViewModel : ObservableObject
    {
        private Color defaultColor = Color.FromRgb(211, 211, 211);
        private Color selectedColor = Color.FromRgb(255, 165, 0);
        private NewFriendlyMatchModel newFriendlyMatch = new NewFriendlyMatchModel();
        private SignalRService signalR = new SignalRService();

        [ObservableProperty]
        private bool isChecked = false;
        [ObservableProperty]
       private bool isCheckedPrivate  = false;
        [ObservableProperty]
       private bool visible = false;
        [ObservableProperty]
        private string pwd ;
        public Color BtnSetColor { get; set; } = Color.FromRgb(211, 211, 211);
        public Color BtnLegColor { get; set; } = Color.FromRgb(211, 211, 211);

        public Color FirstToBtnColor { get; set; } = Color.FromRgb(211, 211, 211);

        public Color BestOfBtnColor { get; set; } = Color.FromRgb(211, 211, 211);

        public Color Btn301Color { get; set; } = Color.FromRgb(211, 211, 211);

        public Color Btn501Color { get; set; } = Color.FromRgb(211, 211, 211);

        public Color Btn701Color { get; set; } = Color.FromRgb(211, 211, 211);

        [ObservableProperty]
        public int numberOfSetsOrLegs;





        private Color RecolorButton(Button BTN)
        {
            var color = BTN.BackgroundColor;
            if (color.Equals(defaultColor))            
                return selectedColor;            
            else            
                return defaultColor;  
        }

        [RelayCommand]
        private void SetsLegsCheck(Button BTN)
        {
            switch (BTN.Text)
            {
                case "Legs":
                    BtnLegColor = RecolorButton(BTN);
                    OnPropertyChanged("BtnLegColor");
                    BtnSetColor = defaultColor;
                    OnPropertyChanged("BtnSetColor");
                   
                    break;
                case "Sets":
                    BtnSetColor = RecolorButton(BTN);
                    OnPropertyChanged("BtnSetColor");
                    BtnLegColor = defaultColor;
                    OnPropertyChanged("BtnLegColor");
                    break;
            }
        }

        [RelayCommand]
        private void FirstToOrBestof(Button BTN)
        {
            switch (BTN.Text)
            {
                case "First to":
                    FirstToBtnColor = RecolorButton(BTN);
                    OnPropertyChanged("FirstToBtnColor");
                    BestOfBtnColor = defaultColor;
                    OnPropertyChanged("BestOfBtnColor");

                    break;
                case "Best of":
                    BestOfBtnColor = RecolorButton(BTN);
                    OnPropertyChanged("BestOfBtnColor");
                    FirstToBtnColor = defaultColor;
                    OnPropertyChanged("FirstToBtnColor");
                    break;
            }
        }

        [RelayCommand]
        private void PointChanging(Button BTN)
        {
            switch (BTN.Text)
            {
                case "301":
                    Btn301Color = RecolorButton(BTN);
                    OnPropertyChanged("Btn301Color");
                    Btn501Color = defaultColor;
                    OnPropertyChanged("Btn501Color");
                    Btn701Color = defaultColor;
                    OnPropertyChanged("Btn701Color");

                    break;
                case "501":
                    Btn501Color = RecolorButton(BTN);
                    OnPropertyChanged("Btn501Color");
                    Btn301Color = defaultColor;
                    OnPropertyChanged("Btn301Color");
                    Btn701Color = defaultColor;
                    OnPropertyChanged("Btn701Color");
                    break;
                case "701":
                    Btn701Color = RecolorButton(BTN);
                    OnPropertyChanged("Btn701Color");
                    Btn301Color = defaultColor;
                    OnPropertyChanged("Btn301Color");
                    Btn501Color = defaultColor;
                    OnPropertyChanged("Btn501Color");
                    break;
            }
        }

        [RelayCommand]
         private  void CheckMatchIsPrivate()
        {
            if (IsCheckedPrivate == true)
            {
                Visible = true;
            }
            else
            {
                Visible = false;
            }
        }

        
        [RelayCommand]

        private async void Navigate()
        {
            List<Color> colors = new List<Color>() { BtnSetColor, BtnLegColor, BestOfBtnColor, FirstToBtnColor, Btn301Color,Btn501Color, Btn701Color};


            if (BtnSetColor == selectedColor)
            {
                if (FirstToBtnColor == selectedColor)
                {
                    newFriendlyMatch.setsCount = NumberOfSetsOrLegs;
                    newFriendlyMatch.legsCount = 3;
                }
                else if(BestOfBtnColor == selectedColor)
                {
                    newFriendlyMatch.setsCount = (int)Math.Floor(NumberOfSetsOrLegs / 2.00) + 1;
                    newFriendlyMatch.legsCount = 3;
                }
            }
            else if (BtnLegColor == selectedColor)
            {
                if (FirstToBtnColor == selectedColor)
                {
                    newFriendlyMatch.legsCount = NumberOfSetsOrLegs;
                }
                else if (BestOfBtnColor == selectedColor)
                {
                    newFriendlyMatch.legsCount = (int)Math.Floor(NumberOfSetsOrLegs/2.00);
                }
                newFriendlyMatch.setsCount = null;
            }
            if (Btn301Color == selectedColor)
            {
                newFriendlyMatch.startingPoint = 301;
            }
            else if(Btn501Color == selectedColor)
            {
                newFriendlyMatch.startingPoint = 501;
            }
            else if (Btn701Color == selectedColor)
            {
                newFriendlyMatch.startingPoint = 701;
            }

            if (IsChecked == true)
            {
                newFriendlyMatch.levelLocked = true;
            }
            else
            {
                newFriendlyMatch.levelLocked = false;
            }
            if (Visible == true)
            {
                newFriendlyMatch.joinPassword = Pwd;
            }
            else
            {
                newFriendlyMatch.joinPassword = null;
            }


            var jsonContent = JsonSerializer.Serialize(newFriendlyMatch);
            var friedlyMatch = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseJson = DartsAPI.PostNewFriendlyMatch(friedlyMatch);

            var response = JsonSerializer.Deserialize<NewFriendlyMatchResponse>(responseJson);

            if (response != null)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync($"//{nameof(FriendlyMatchPage)}");
                });
                WaitingForPlayersPopUp popUp = new WaitingForPlayersPopUp();
                Application.Current.MainPage.ShowPopup(popUp);
                await signalR.ConnectAsync(SecStoreItems.AToken);

            
                await signalR.JoinFriendlyMatch(response.matchId, SecStoreItems.UserId, SecStoreItems.UserName, SecStoreItems.DartsPoints);

                signalR.OnFriendlyPlayerJoined += async (playerId, username, dartsPoint) =>
                {
                    var popupVm = new JoinRequestPopUpViewModel(signalR, response.matchId, playerId, username, dartsPoint);
                    var popupPage = new JoinRequestPopUp(popupVm, response.matchId);

                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Application.Current.MainPage.ShowPopupAsync(popupPage); // vagy PushPopupAsync
                    });
                };
            }
            else
            {
                Debug.WriteLine($"\n\n\n\n\n{response}\n\n\n\n\n");
            }

        }
    }
}
