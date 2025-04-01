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
        public Button LegsBtn { get; set; } = new();
        public Button SetsBtn { get; set; } = new();

        public Button FirstToBtn { get; set; }

        private void RecolorButton(Button BTN)
        {
            var color = BTN.BackgroundColor;
            if (color.Equals(defaultColor))            
                BTN.BackgroundColor = selectedColor;            
            else            
                BTN.BackgroundColor = defaultColor;  
        }


        [RelayCommand]
        private void SetsLegsCheck(Button BTN)
        {
            switch (BTN.Text)
            {
                case "Legs":
                    RecolorButton(BTN);
                    SetsBtn.BackgroundColor = defaultColor;
                    break;
                case "Sets":
                    RecolorButton(BTN);
                    LegsBtn.BackgroundColor = defaultColor;
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
