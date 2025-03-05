using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class FriendlySetupViewModel : ObservableObject
    {
        private Color defaultColor = Color.FromRgb(211, 211, 211);
        private Color selectedColor = Color.FromRgb(255, 165, 0);
       

        [RelayCommand]
        private void RecolorButton(Button BTN)
        {
            var color = BTN.BackgroundColor;
            if (color.Equals(defaultColor))            
                BTN.BackgroundColor = selectedColor;            
            else            
                BTN.BackgroundColor = defaultColor;            
        }

    }
}
