using CommunityToolkit.Maui.Views;
using DartsMobilApp.ViewModel;
using System.Drawing;

namespace DartsMobilApp.Pages;

public partial class EndGameCheckOutPopUp : Popup
{
    int DartsOnDouble = 0;
    int DartsInCheckOut = 0;

    public EndGameCheckOutPopUp()
	{
		InitializeComponent();
	}

    private void AllDartsInCheckOutClicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        DartsInCheckOut = int.Parse(button.Text);
    }

    private void AllDartsOnDoubleClicked(object sender, EventArgs e)
    {
        Button Btn = sender as Button;
        DartsOnDouble = int.Parse(Btn.Text);

    }

    private void SendParameters(object sender, EventArgs e)
    {
        CounterViewModel.MyPlayerAllThrownDarts += DartsInCheckOut;
        CounterViewModel.MyPlayerAllThrownDartsForDouble += DartsOnDouble;
        CounterViewModel.MyPlayerSuccessfulThrownDartsForDouble++;
        this.CloseAsync();
    }
}