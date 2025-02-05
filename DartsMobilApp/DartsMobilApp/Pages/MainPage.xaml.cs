using DartsMobilApp.ViewModel;
using Microsoft.Maui.Controls;

namespace DartsMobilApp.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}