using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class FriendlyMatchPage : ContentPage
{
	public FriendlyMatchPage(FriendlyMatchViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}