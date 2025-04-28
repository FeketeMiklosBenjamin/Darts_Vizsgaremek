using CommunityToolkit.Maui.Views;
using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class SimpleCheckoutPopUp : Popup
{
	public SimpleCheckoutPopUp()
	{
		InitializeComponent();
	}

    private void Darts_CheckOutClicked(object sender, EventArgs e)
    {
        var SelectedDartsCount = 0;
        Button Btn = sender as Button;
        if (int.TryParse(Btn.Text, out int count))
        {
            SelectedDartsCount = count;
        }
        CounterViewModel.MyPlayerAllThrownDarts += 3;
        CounterViewModel.MyPlayerAllThrownDartsForDouble += SelectedDartsCount;
        this.Close();
    }
}