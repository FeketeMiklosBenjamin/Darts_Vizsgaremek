using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class StatisticPage : ContentPage
{
	public StatisticPage(StatisticViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}