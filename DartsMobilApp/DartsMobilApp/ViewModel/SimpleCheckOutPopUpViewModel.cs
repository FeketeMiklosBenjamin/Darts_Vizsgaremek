using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class SimpleCheckOutPopUpViewModel :ObservableObject
    {
        [ObservableProperty]
        private int selectedDartsCount;

        [RelayCommand]
        private void SelectDarts(string NumberOfDarts)
        {
            if (int.TryParse(NumberOfDarts, out int count))
            {
                SelectedDartsCount = count;
            }
            CounterViewModel.MyPlayerAllThrownDarts += 3;
            CounterViewModel.MyPlayerAllThrownDartsForDouble += SelectedDartsCount;
        }

    }
}
