using CommunityToolkit.Maui.Views;

namespace DartsMobilApp.Pages;

public partial class HostRejectPopUp : Popup
{
    public HostRejectPopUp()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}