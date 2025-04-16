using CommunityToolkit.Maui.Views;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using DartsMobilApp.Classes;
using DartsMobilApp.API;
using DartsMobilApp.Services;
using DartsMobilApp.SecureStorageItems;


namespace DartsMobilApp.Pages;

public partial class PasswordValidationPopUp : Popup
{
    private string MatchId;

    private readonly SignalRService _signalRService;
	public PasswordValidationPopUp(SignalRService rService, string matchID)
	{
		InitializeComponent();
        _signalRService = rService;
        MatchId = matchID;
	}
    
   
        

    private async void CheckvalidPassword(object sender, EventArgs e)
    {
        string password = PwdEntry.Text;
        ValidatePassword ValidatePassword = new ValidatePassword()
        {
            password = password
        };
        var jsonContent = JsonSerializer.Serialize(ValidatePassword);
        var response = DartsAPI.ValidatePassword(new StringContent(jsonContent, Encoding.UTF8, "application/json"), MatchId);
        if (response.message == "Sikeres belépés!")
        {
            WaitingForPlayersPopUp popUp = new WaitingForPlayersPopUp(_signalRService, MatchId);
            Application.Current?.MainPage?.ShowPopup(popUp);
            _signalRService.JoinTournamentMatch(MatchId, SecStoreItems.UserId);
            this.Close();
        }
    }
}