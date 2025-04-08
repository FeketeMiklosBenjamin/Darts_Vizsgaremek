using CommunityToolkit.Maui.Views;
using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class JoinRequestPopUp : Popup
{
	public JoinRequestPopUp(JoinRequestPopUpViewModel vm, string matchId)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}