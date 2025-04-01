using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class FriendlySetupViewModel : ObservableObject
    {
        private Color defaultColor = Color.FromRgb(211, 211, 211);
        private Color selectedColor = Color.FromRgb(255, 165, 0);
        private FriendlyMatchModel newFriendlyMatch = new FriendlyMatchModel();


        public Color BtnSetColor { get; set; } = Color.FromRgb(211, 211, 211);
        public Color BtnLegColor { get; set; } = Color.FromRgb(211, 211, 211);

        public Color FirstToBtnColor { get; set; } = Color.FromRgb(211, 211, 211);

        public Color BestOfBtnColor { get; set; } = Color.FromRgb(211, 211, 211);

        public Color Btn301Color { get; set; } = Color.FromRgb(211, 211, 211);

        public Color Btn501Color { get; set; } = Color.FromRgb(211, 211, 211);

        public Color Btn701Color { get; set; } = Color.FromRgb(211, 211, 211);
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

        private async void Navigate()
        {
           newFriendlyMatch.name = "Test";
            newFriendlyMatch.playerLevel = "Amatour";
            newFriendlyMatch.levelLocked = false;
            newFriendlyMatch.setsCount = 0;
            newFriendlyMatch.legsCount = 6;
            newFriendlyMatch.startingPoint = 501;
            newFriendlyMatch.joinPassword = "1123581321345589144233377";
            var jsonContent = JsonSerializer.Serialize(newFriendlyMatch);
            var friedlyMatch = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            //var response = DartsAPI.PostNewFriendlyMatch(friedlyMatch);
            
            //if (response.StatusCode == 204)
            //{
            //    MainThread.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Shell.Current.GoToAsync($"//{nameof(FriendlyMatchPage)}");
            //    });
            //}
            //else
            //{
            //    Debug.WriteLine($"\n\n\n\n\n{response.StatusCode} - {response.Message}\n\n\n\n\n");
            //}

        }
    }
}
