using CommunityToolkit.Maui.Views;

namespace DartsMobilApp.Pages;

public partial class DisconnectedPopUp : Popup
{
    public DisconnectedPopUp()
    {
        InitializeComponent();
    }

    private async void Disconnect(object sender, EventArgs e)
    {
        await Shell.Current.Dispatcher.DispatchAsync(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        });
        this.Close();
    }
}