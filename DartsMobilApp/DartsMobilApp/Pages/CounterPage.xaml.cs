using CommunityToolkit.Maui.Views;
using DartsMobilApp.Classes;
using DartsMobilApp.Services;
using DartsMobilApp.ViewModel;

namespace DartsMobilApp.Pages;

public partial class CounterPage : ContentPage
{
    private readonly SignalRService _signalRService;
    public CounterPage(CounterViewModel vm, SignalRService signalRService)
	{
		InitializeComponent();
		this.BindingContext = vm;
		counterGrid = vm.counterGrid;
        _signalRService = signalRService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _signalRService.OnOpponentDisconnected += OnOpponentDisconnectedHandler;
        _signalRService.OnEndMatchResult += OnEndMatchResultHandler;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _signalRService.OnOpponentDisconnected -= OnOpponentDisconnectedHandler;
        _signalRService.OnEndMatchResult -= OnEndMatchResultHandler;
    }

    private async void OnOpponentDisconnectedHandler()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            DisconnectedPopUp disconnectedPopUp = new DisconnectedPopUp();
            await Application.Current.MainPage.ShowPopupAsync(disconnectedPopUp);
        });
    }

    private async void OnEndMatchResultHandler(EndMatchModel playerOneStat, EndMatchModel playerTwoStat)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var counterViewModel = new CounterViewModel(_signalRService);

            var matchStatisticPopUp = new MatchStatisticPopUp(
                playerOneStat,
                playerTwoStat,
                counterViewModel.StartingPlayerName,
                counterViewModel.SecondPlayerName,
                counterViewModel.ImTheFirst
            );

            await Application.Current.MainPage.ShowPopupAsync(matchStatisticPopUp);
        });
    }
}