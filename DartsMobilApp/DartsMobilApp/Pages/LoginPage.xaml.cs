using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

   
}