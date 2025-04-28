using DartsMobilApp.ViewModel;
using Microsoft.Maui.Controls;

namespace DartsMobilApp.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}