using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class FriendlySetupViewModel : ObservableObject
    {
        [RelayCommand]

        private void RecolorButton(Button BTN)
        {
            if (BTN.BackgroundColor == Color.FromRgb(211,211,211))
            {
                BTN.BackgroundColor = Color.FromRgb(255, 165, 0);
            }
            else
            {
                BTN.BackgroundColor = Color.FromRgb(211, 211, 211);
            }
        }
        
    }
}
