using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Microsoft.Maui.Controls;

namespace DartsMobilApp.ViewModel
{
    public partial class CounterViewModel : ObservableObject
    {
        [ObservableProperty]
        public Grid? counterGrid;


        [ObservableProperty]
        public string points;

        [ObservableProperty]

        public string pointsFirstPlayer = "501"; 

        [ObservableProperty]

        public string pointsSecondPlayer = "501";


        [RelayCommand]
        private void Appearing()
        {
        }

        [RelayCommand]

        private void AddPoints(string number) 
        {
            Points += number;
        }

        bool isFirstPlayer = true;

        [RelayCommand]

        private void SendPoints(string point) 
        {
            
            if (!isFirstPlayer)
            {
                PointsSecondPlayer = (int.Parse(PointsSecondPlayer) - int.Parse(point)).ToString();
                Points = "";
                isFirstPlayer = true;
            }
            else
            {
                
                PointsFirstPlayer = (int.Parse(PointsFirstPlayer) - int.Parse(point)).ToString();
                Points = "";
                isFirstPlayer = false;
            }
        }

    }
}
