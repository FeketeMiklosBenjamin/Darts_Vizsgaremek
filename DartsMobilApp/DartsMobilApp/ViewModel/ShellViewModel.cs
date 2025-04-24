using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;

namespace DartsMobilApp.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userDisplayName;

        public ShellViewModel()
        {
            LoadUserDisplayName();
        }

        public async void LoadUserDisplayName()
        {
            var username = await SecureStorage.Default.GetAsync("UserName");
            UserDisplayName = username;
        }
    }
}