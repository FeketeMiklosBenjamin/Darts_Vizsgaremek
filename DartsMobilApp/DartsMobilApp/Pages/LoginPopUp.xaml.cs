using CommunityToolkit.Maui.Views;

namespace DartsMobilApp.Pages;

public partial class LoginPopUp : Popup
{
	public LoginPopUp()
	{
		InitializeComponent();
	}
	

    private void LoginOK(object sender, EventArgs e)
    {
        this.Close();
    }
}