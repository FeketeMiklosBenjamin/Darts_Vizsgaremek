using CommunityToolkit.Maui.Views;
using DartsMobilApp.ViewModel;

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
        UpdateButtonColor(button);
    }

    private void AllDartsOnDoubleClicked(object sender, EventArgs e)
    {
        Button Btn = sender as Button;
        DartsOnDouble = int.Parse(Btn.Text);
        UpdateButtonColor(Btn);
    }

    private void UpdateButtonColor(Button clickedButton)
    {
        var parent = clickedButton.Parent;

        if (parent is HorizontalStackLayout horizontalStackLayout)
        {
            foreach (var child in horizontalStackLayout.Children)
            {
                if (child is Button button)
                {
                    button.BackgroundColor = Colors.White;
                }
            }
        }
        clickedButton.BackgroundColor = (Color)Application.Current.Resources["Primary"];
    }


    private void SendParameters(object sender, EventArgs e)
    {
        CounterViewModel.MyPlayerAllThrownDarts += DartsInCheckOut;
        CounterViewModel.MyPlayerAllThrownDartsForDouble += DartsOnDouble;
        CounterViewModel.MyPlayerSuccessfulThrownDartsForDouble++;
        this.CloseAsync();
    }
}
