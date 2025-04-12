using DartsMobilApp.Classes;
using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class CounterPage : ContentPage
{
	public CounterPage(CounterViewModel vm)
	{

		InitializeComponent();
		this.BindingContext = vm;
		counterGrid = vm.counterGrid;

    }
}