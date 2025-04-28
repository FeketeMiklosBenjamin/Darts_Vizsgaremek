using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class CompetitionsPage : ContentPage
{
	public CompetitionsPage(CompetitionsViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}