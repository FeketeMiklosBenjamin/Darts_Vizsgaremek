using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class FriendlySetupPage : ContentPage
{
	public FriendlySetupPage(FriendlySetupViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}