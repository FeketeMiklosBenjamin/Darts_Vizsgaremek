using CommunityToolkit.Maui.Views;
using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class SimpleCheckoutPopUp : Popup
{
	public SimpleCheckoutPopUp(SimpleCheckOutPopUpViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}