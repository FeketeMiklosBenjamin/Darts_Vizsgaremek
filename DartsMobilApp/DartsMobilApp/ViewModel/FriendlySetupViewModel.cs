using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsMobilApp.API;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Services;
using DartsMobilApp.ViewModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;


public partial class FriendlySetupViewModel : ObservableObject
{
    private readonly SignalRService _signalR;
    private Color defaultColor = Color.FromRgb(211, 211, 211);
    private Color selectedColor = Color.FromRgb(255, 165, 0);
    private NewFriendlyMatchModel newFriendlyMatch = new NewFriendlyMatchModel();

    public FriendlySetupViewModel(SignalRService signalR)
    {
        _signalR = signalR;
    }

    private string currentMatchId;

    [ObservableProperty] private bool isChecked;
    [ObservableProperty] private bool isCheckedPrivate;
    [ObservableProperty] private bool visible;
    [ObservableProperty] private string pwd;
    [ObservableProperty] public int? numberOfSetsOrLegs;

    [ObservableProperty] private Color btnSetColor;
    [ObservableProperty] private Color btnLegColor;
    [ObservableProperty] private Color firstToBtnColor;
    [ObservableProperty] private Color bestOfBtnColor;
    [ObservableProperty] private Color btn301Color;
    [ObservableProperty] private Color btn501Color;
    [ObservableProperty] private Color btn701Color;

    [RelayCommand]
    private async Task Appearing()
    {
        BtnSetColor = defaultColor;
        BtnLegColor = selectedColor;
        FirstToBtnColor = selectedColor;
        BestOfBtnColor = defaultColor;
        Btn301Color = defaultColor;
        Btn501Color = selectedColor;
        Btn701Color = defaultColor;
        NumberOfSetsOrLegs = 2;
    }

    [RelayCommand]
    private void SetsLegsCheck(Button BTN)
    {
        BtnSetColor = defaultColor;
        BtnLegColor = defaultColor;

        if (BTN.Text == "Legs")
            BtnLegColor = selectedColor;
        else if (BTN.Text == "Sets")
            BtnSetColor = selectedColor;
    }

    [RelayCommand]
    private void FirstToOrBestof(Button BTN)
    {
        FirstToBtnColor = defaultColor;
        BestOfBtnColor = defaultColor;

        if (BTN.Text == "First to")
            FirstToBtnColor = selectedColor;
        else if (BTN.Text == "Best of")
            BestOfBtnColor = selectedColor;
    }


    [RelayCommand]
    private void PointChanging(Button BTN)
    {
        Btn301Color = defaultColor;
        Btn501Color = defaultColor;
        Btn701Color = defaultColor;

        if (BTN.Text == "301")
            Btn301Color = selectedColor;
        else if (BTN.Text == "501")
            Btn501Color = selectedColor;
        else if (BTN.Text == "701")
            Btn701Color = selectedColor;
    }


    [RelayCommand]
    private void CheckMatchIsPrivate()
    {
        Visible = IsCheckedPrivate;
    }

    [RelayCommand]
    private async Task Navigate()
    {
        try
        {
            if (await SetMatchParameters() == false)
            {
                return;
            }

            var jsonContent = JsonSerializer.Serialize(newFriendlyMatch);
            var responseJson = await DartsAPI.PostNewFriendlyMatch(new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            var response = JsonSerializer.Deserialize<NewFriendlyMatchResponse>(responseJson);

            if (response == null)
            {
                await Application.Current.MainPage.DisplayAlert("Hiba", "Nem sikerült létrehozni a meccset.", "Ok");
                return;
            }

            currentMatchId = response.matchId;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(FriendlyMatchPage)}");
                Application.Current.MainPage.ShowPopup(new WaitingForPlayersPopUp(_signalR, currentMatchId));
            });

            await _signalR.JoinFriendlyMatch(currentMatchId, SecStoreItems.UserId, SecStoreItems.UserName, SecStoreItems.DartsPoints);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigate error: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Hiba", "Valami hiba történt.", "Ok");
        }
    }

    private async Task<bool> SetMatchParameters()
    {
        if (NumberOfSetsOrLegs == null || NumberOfSetsOrLegs <= 0)
        {
            await Application.Current.MainPage.DisplayAlert("Hiba", "Adj meg érvényes számot a szettek/legek számához!", "Ok");
            return false;
        }

        if (BtnSetColor == selectedColor)
        {
            newFriendlyMatch.setsCount = FirstToBtnColor == selectedColor
                ? NumberOfSetsOrLegs
                : (int)Math.Floor((double)NumberOfSetsOrLegs / 2.0) + 1;
            newFriendlyMatch.legsCount = 3;
        }
        else if (BtnLegColor == selectedColor)
        {
            newFriendlyMatch.legsCount = FirstToBtnColor == selectedColor
                ? NumberOfSetsOrLegs
                : (int)Math.Floor((double)NumberOfSetsOrLegs / 2.0) + 1;
            newFriendlyMatch.setsCount = null;
        }

        newFriendlyMatch.startingPoint = Btn301Color == selectedColor ? 301 :
                                         Btn501Color == selectedColor ? 501 :
                                         Btn701Color == selectedColor ? 701 : 501;

        newFriendlyMatch.levelLocked = IsChecked;
        newFriendlyMatch.joinPassword = Visible ? Pwd : null;
        return true;
    }
}
