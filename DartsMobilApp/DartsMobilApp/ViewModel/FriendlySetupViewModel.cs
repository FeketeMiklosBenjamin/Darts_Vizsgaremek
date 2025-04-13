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
    [ObservableProperty] public int numberOfSetsOrLegs;

    public Color BtnSetColor { get; set; } = Color.FromRgb(211, 211, 211);
    public Color BtnLegColor { get; set; } = Color.FromRgb(211, 211, 211);
    public Color FirstToBtnColor { get; set; } = Color.FromRgb(211, 211, 211);
    public Color BestOfBtnColor { get; set; } = Color.FromRgb(211, 211, 211);
    public Color Btn301Color { get; set; } = Color.FromRgb(211, 211, 211);
    public Color Btn501Color { get; set; } = Color.FromRgb(211, 211, 211);
    public Color Btn701Color { get; set; } = Color.FromRgb(211, 211, 211);

    private Color RecolorButton(Button BTN)
    {
        return BTN.BackgroundColor.Equals(defaultColor) ? selectedColor : defaultColor;
    }

    [RelayCommand]
    private void SetsLegsCheck(Button BTN)
    {
        if (BTN.Text == "Legs")
        {
            BtnLegColor = RecolorButton(BTN);
            OnPropertyChanged(nameof(BtnLegColor));
            BtnSetColor = defaultColor;
            OnPropertyChanged(nameof(BtnSetColor));
        }
        else if (BTN.Text == "Sets")
        {
            BtnSetColor = RecolorButton(BTN);
            OnPropertyChanged(nameof(BtnSetColor));
            BtnLegColor = defaultColor;
            OnPropertyChanged(nameof(BtnLegColor));
        }
    }

    [RelayCommand]
    private void FirstToOrBestof(Button BTN)
    {
        if (BTN.Text == "First to")
        {
            FirstToBtnColor = RecolorButton(BTN);
            OnPropertyChanged(nameof(FirstToBtnColor));
            BestOfBtnColor = defaultColor;
            OnPropertyChanged(nameof(BestOfBtnColor));
        }
        else if (BTN.Text == "Best of")
        {
            BestOfBtnColor = RecolorButton(BTN);
            OnPropertyChanged(nameof(BestOfBtnColor));
            FirstToBtnColor = defaultColor;
            OnPropertyChanged(nameof(FirstToBtnColor));
        }
    }

    [RelayCommand]
    private void PointChanging(Button BTN)
    {
        Btn301Color = defaultColor;
        Btn501Color = defaultColor;
        Btn701Color = defaultColor;

        switch (BTN.Text)
        {
            case "301": Btn301Color = RecolorButton(BTN); break;
            case "501": Btn501Color = RecolorButton(BTN); break;
            case "701": Btn701Color = RecolorButton(BTN); break;
        }

        OnPropertyChanged(nameof(Btn301Color));
        OnPropertyChanged(nameof(Btn501Color));
        OnPropertyChanged(nameof(Btn701Color));
    }

    [RelayCommand]
    private void CheckMatchIsPrivate()
    {
        Visible = IsCheckedPrivate;
    }

    [RelayCommand]
    private async Task Navigate()
    {
        if (BtnSetColor == selectedColor)
        {
            newFriendlyMatch.setsCount = FirstToBtnColor == selectedColor
                ? NumberOfSetsOrLegs
                : (int)Math.Floor(NumberOfSetsOrLegs / 2.0) + 1;
            newFriendlyMatch.legsCount = 3;
        }
        else if (BtnLegColor == selectedColor)
        {
            newFriendlyMatch.legsCount = FirstToBtnColor == selectedColor
                ? NumberOfSetsOrLegs
                : (int)Math.Floor(NumberOfSetsOrLegs / 2.0) + 1;
            newFriendlyMatch.setsCount = null;
        }

        newFriendlyMatch.startingPoint = Btn301Color == selectedColor ? 301 :
                                         Btn501Color == selectedColor ? 501 :
                                         Btn701Color == selectedColor ? 701 : 501;

        newFriendlyMatch.levelLocked = IsChecked;
        newFriendlyMatch.joinPassword = Visible ? Pwd : null;

        var jsonContent = JsonSerializer.Serialize(newFriendlyMatch);
        var responseJson = DartsAPI.PostNewFriendlyMatch(new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        var response = JsonSerializer.Deserialize<NewFriendlyMatchResponse>(responseJson);

        if (response != null)
        {
            currentMatchId = response.matchId;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(FriendlyMatchPage)}");
            });

            Application.Current.MainPage.ShowPopup(new WaitingForPlayersPopUp(_signalR, currentMatchId));
            await _signalR.JoinFriendlyMatch(response.matchId, SecStoreItems.UserId, SecStoreItems.UserName, SecStoreItems.DartsPoints);
        }
        else
        {
            Debug.WriteLine("Nem sikerült létrehozni a barátságos meccset.");
        }
    }
}
