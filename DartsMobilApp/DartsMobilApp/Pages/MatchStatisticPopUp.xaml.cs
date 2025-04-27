using CommunityToolkit.Maui.Views;
using DartsMobilApp.Classes;

namespace DartsMobilApp.Pages;

public partial class MatchStatisticPopUp : Popup
{
    public EndMatchModel PlayerOneStat { get; set; }
    public EndMatchModel PlayerTwoStat { get; set; }

    public string StartingPlayerName { get; set; }
    public string SecondPlayerName { get; set; }

    public int? StartingPlayerScore { get; set; }
    public int? SecondPlayerScore { get; set; }

    public bool IsSets { get; set; } = false;

    private bool ImTheFirst { get; set; }

    public MatchStatisticPopUp()
    {
        InitializeComponent();
        BindingContext = this;
    }


    public MatchStatisticPopUp(EndMatchModel playerOneStat, EndMatchModel playerTwoStat, string startingPlayerName, string secondPlayerName, bool imTheFirst)
    {
        InitializeComponent();
        PlayerOneStat = playerOneStat;
        PlayerTwoStat = playerTwoStat;
        ImTheFirst = imTheFirst;
        if (ImTheFirst)
        {
            StartingPlayerName = startingPlayerName;
            SecondPlayerName = secondPlayerName;
        }
        else
        {
            StartingPlayerName = secondPlayerName;
            SecondPlayerName = startingPlayerName;
        }
        if (playerOneStat.Sets == 0 && playerTwoStat.Sets == 0)
        {
            IsSets = true;
            SecondPlayerScore = playerTwoStat.Legs;
            StartingPlayerScore = playerOneStat.Legs;
        }
        else
        {
            SecondPlayerScore = playerTwoStat.Sets;
            StartingPlayerScore = playerOneStat.Sets;
        }

        BindingContext = this;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.Close();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        });
    }
}