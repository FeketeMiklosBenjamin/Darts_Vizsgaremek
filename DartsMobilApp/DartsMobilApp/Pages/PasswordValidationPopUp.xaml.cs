using CommunityToolkit.Maui.Views;
using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class PasswordValidationPopUp : Popup
{
	public PasswordValidationPopUp(PasswordValidationPopUpViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}