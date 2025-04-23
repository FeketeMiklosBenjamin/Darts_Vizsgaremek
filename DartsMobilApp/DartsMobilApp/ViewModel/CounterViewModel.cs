using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using DartsMobilApp.Classes;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using DartsMobilApp.Services;
using DartsMobilApp.SecureStorageItems;
using DartsMobilApp.Pages;
using System.Threading.Tasks;
using DartsMobilApp.API;
using CommunityToolkit.Maui.Views;

namespace DartsMobilApp.ViewModel
{

    public partial class CounterViewModel : ObservableObject
    {

        private readonly SignalRService _signalRService;

        public static StartFriendlyMatchModel settings;


        [ObservableProperty]
        public Grid? counterGrid;

        [ObservableProperty]
        public string recommendedFirstCheckout;
        [ObservableProperty]
        public string recommendedSecondCheckout;

        [ObservableProperty]
        public string startingPlayerName;

        [ObservableProperty]
        public string secondPlayerName;

        [ObservableProperty]
        public int needToWinSets;


        [ObservableProperty]
        public int firstPlayerSetsWon;

        [ObservableProperty]
        public int secondPlayerSetsWon;

        public int allPlayedSet
        {
            get
            {
                return firstPlayerSetsWon + secondPlayerSetsWon;
            }
        }

        private int MyPlayer180s = 0;



        private double MyPlayerPoints = 0.0;



        private List<int> MyLegScores = new List<int>();



        public static int MyPlayerAllThrownDarts = 0;


        public static int MyPlayerAllThrownDartsForDouble = 0;


        private int MyPlayerSuccessfulThrownDartsForDouble = 0;

        private int MyNineDarters = 0;


        private double MyAverage {
            get
            {
                return Math.Round(MyPlayerPoints / (MyPlayerAllThrownDarts / 3),2);
            }
        }

        [ObservableProperty]
        public int needToWinLegs;

        public static string MatchId;

        [ObservableProperty]
        public int firstPlayerWonLeg;

        private bool isFirstPlayer = true;

        


        [ObservableProperty]
        public int secondPlayerWonLeg;

        public int allPlayedLegs
        {
            get
            {
                return secondPlayerWonLeg + firstPlayerWonLeg;
            }
        }

        public int AllWonLegsOnTheMatchSecond = 0;

        private int AllPlayedLegsOnTheMatch = 0;

        [ObservableProperty]
        public string points;

        [ObservableProperty]

        public string pointsFirstPlayer; 

        [ObservableProperty]

        public string pointsSecondPlayer;

        private bool ImTheFirst;

        [ObservableProperty]
        private bool enabledButton;

        private int AllWonLegsOnTheMatchFirst = 0;

        public static bool IsFriendlyMatch;


        private int MyHighestCheckOut = 0;


        public CounterViewModel(SignalRService signalR)
        {
            _signalRService = signalR;
        }

        
        [RelayCommand]
        private void Appearing()
        {
            CheckStartingPlayer();
            PointsFirstPlayer = PointsSecondPlayer = settings.StartingPoint.ToString();
            NeedToWinLegs = settings.SetCount > 1 ? NeedToWinLegs = 3 : NeedToWinLegs = settings.LegCount;
            NeedToWinSets = settings.SetCount;
            StartingPlayerName = settings.PlayerOneName;
            SecondPlayerName = settings.PlayerTwoName;
            SecondPlayerWonLeg = FirstPlayerWonLeg = 0;
            _signalRService.OnGetPoints += async (OpponentPoint) =>
            {
                if (ImTheFirst)
                {
                    SetSecondPlayersPoints(OpponentPoint.ToString());
                    TextSpeach(OpponentPoint.ToString());
                    CheckMatchWinner();

                }
                else
                {
                    SetFirstPlayersPoints(OpponentPoint.ToString());
                    TextSpeach(OpponentPoint.ToString());
                    CheckMatchWinner();

                }
            };
        }

        private void CheckStartingPlayer()
        {
            if (settings.StartingPlayer == SecStoreItems.UserId)
            {
                ImTheFirst = true;
                EnabledButton = true;
            }
            else
            {
                ImTheFirst = false;
                EnabledButton = false;
            }
        }


        [RelayCommand]

        private void AddPoints(string number) 
        {
            Points += number;
        }

        Dictionary<int, string> checkOutTable = new Dictionary<int, string>()
        {
            {170, "T20 T20 Bull"},
            {169, "No checkout"},
            {168, "No checkout"},
            {167, "T20 T19 Bull"},
            {166, "No checkout"},
            {165, "No checkout"},
            {164, "T20 T18 Bull"},
            {163, "No checkout"},
            {162, "No checkout"},
            {161, "T20 T17 Bull"},
            {160, "T20 T20 D20"},
            {159, "No checkout"},
            {158, "T20 T20 D19"},
            {157, "T20 T19 D20"},
            {156, "T20 T20 D18"},
            {155, "T20 T19 D19"},
            {154, "T20 T18 D20"},
            {153, "T20 T19 D18"},
            {152, "T20 T20 D16"},
            {151, "T20 T17 D20"},
            {150, "T20 T18 D18"},
            {149, "T20 T19 D16"},
            {148, "T20 T20 D14"},
            {147, "T20 T17 D18"},
            {146, "T20 T18 D16"},
            {145, "T20 T15 D20"},
            {144, "T20 T20 D12"},
            {143, "T20 T17 D16"},
            {142, "T20 T14 D20"},
            {141, "T20 T19 D12"},
            {140, "T20 T20 D10"},
            {139, "T20 T13 D20"},
            {138, "T20 T18 D12"},
            {137, "T20 T19 D10"},
            {136, "T20 T20 D8"},
            {135, "T20 T15 D15"},
            {134, "T20 T14 D16"},
            {133, "T20 T19 D8"},
            {132, "T20 T16 D12"},
            {131, "T20 T13 D16"},
            {130, "T20 T20 D5"},
            {129, "T19 T16 D12"},
            {128, "T18 T14 D16"},
            {127, "T20 T17 D8"},
            {126, "T19 T19 D6"},
            {125, "T20 T15 D10"},
            {124, "T20 T16 D8"},
            {123, "T19 T16 D9"},
            {122, "T18 T18 D7"},
            {121, "T20 T11 D14"},
            {120, "T20 20 D20"},
            {119, "T19 T12 D13"},
            {118, "T20 18 D20"},
            {117, "T20 17 D20"},
            {116, "T20 16 D20"},
            {115, "T20 15 D20"},
            {114, "T20 14 D20"},
            {113, "T20 13 D20"},
            {112, "T20 12 D20"},
            {111, "T20 19 D16"},
            {110, "T20 18 D16"},
            {109, "T20 17 D16"},
            {108, "T20 16 D16"},
            {107, "T19 18 D16"},
            {106, "T20 14 D16"},
            {105, "T20 13 D16"},
            {104, "T18 18 D16"},
            {103, "T19 10 D18"},
            {102, "T20 10 D16"},
            {101, "T17 18 D16"},
            {100, "T20 D20"},
            {99, "T19 10 D16"},
            {98, "T20 D19"},
            {97, "T19 D20"},
            {96, "T20 D18"},
            {95, "T19 D19"},
            {94, "T18 D20"},
            {93, "T19 D18"},
            {92, "T20 D16"},
            {91, "T17 D20"},
            {90, "T20 D15"},
            {89, "T19 D16"},
            {88, "T20 D14"},
            {87, "T17 D18"},
            {86, "T18 D16"},
            {85, "T15 D20"},
            {84, "T20 D12"},
            {83, "T17 D16"},
            {82, "T14 D20"},
            {81, "T19 D12"},
            {80, "T20 D10"},
            {79, "T13 D20"},
            {78, "T18 D12"},
            {77, "T19 D10"},
            {76, "T20 D8"},
            {75, "T17 D12"},
            {74, "T14 D16"},
            {73, "T19 D8"},
            {72, "T16 D12"},
            {71, "T13 D16"},
            {70, "T18 D8"},
            {69, "T19 D6"},
            {68, "T20 D4"},
            {67, "T17 D8"},
            {66, "T10 D18"},
            {65, "T19 D4"},
            {64, "T16 D8"},
            {63, "T13 D12"},
            {62, "T10 D16"},
            {61, "T15 D8"},
            {60, "20 D20"},
            {59, "19 D20"},
            {58, "18 D20"},
            {57, "17 D20"},
            {56, "16 D20"},
            {55, "15 D20"},
            {54, "14 D20"},
            {53, "13 D20"},
            {52, "20 D16"},
            {51, "19 D16"},
            {50, "18 D16"},
            {49, "17 D16"},
            {48, "16 D16"},
            {47, "15 D16"},
            {46, "6 D20"},
            {45, "13 D16"},
            {44, "12 D16"},
            {43, "3 D20"},
            {42, "10 D16"},
            {41, "9 D16"},
            {40, "D20"},
            {39, "7 D16"},
            {38, "D19"},
            {37, "5 D16"},
            {36, "D18"},
            {35, "3 D16"},
            {34, "D17"},
            {33, "1 D16"},
            {32, "D16"},
            {31, "15 D8"},
            {30, "D15"},
            {29, "13 D8"},
            {28, "D14"},
            {27, "11 D8"},
            {26, "D13"},
            {25, "9 D8"},
            {24, "D12"},
            {23, "7 D8"},
            {22, "D11"},
            {21, "5 D8"},
            {20, "D10"},
            {19, "3 D8"},
            {18, "D9"},
            {17, "1 D8"},
            {16, "D8"},
            {15, "7 D4"},
            {14, "D7"},
            {13, "5 D4"},
            {12, "D6"},
            {11, "3 D4"},
            {10, "D5"},
            {9, "1 D4"},
            {8, "D4"},
            {7, "3 D2"},
            {6, "D3"},
            {5, "1 D2"},
            {4, "D2"},
            {3, "1 D1"},
            {2, "D1"},
            {1, "No checkout" }
        };

        ObservableCollection<string> impossibleThrows = new ObservableCollection<string>{ "163", "166", "169", "172", "173", "175", "176", "178", "179" };




        private void SetFirstPlayersPoints(string thrownPoint)
        {
            if (int.Parse(thrownPoint) > int.Parse(PointsFirstPlayer) || int.Parse(PointsFirstPlayer) - int.Parse(thrownPoint) == 1)
            {
                thrownPoint = "0";
            }
            PointsFirstPlayer = (int.Parse(PointsFirstPlayer) - int.Parse(thrownPoint)).ToString();
            if (PointsFirstPlayer == "0")
            {
                if (int.Parse(thrownPoint) > MyHighestCheckOut)
                {
                    MyHighestCheckOut = int.Parse(thrownPoint);
                }
                var SumLegPoints = 0;
                for (int i = 0; i < MyLegScores.Count; i++)
                {
                    SumLegPoints += MyLegScores[i];
                }
                if (MyLegScores.Count == 3 && SumLegPoints == 501)
                {
                    MyNineDarters++;
                }
                AllWonLegsOnTheMatchFirst++;
                FirstPlayerWonLeg++;
                MyLegScores.Clear();
                SetDefaultValues();
                TextSpeach($"{StartingPlayerName} nyerte a {allPlayedLegs}. leget!");
                if (allPlayedLegs % 2 == 0)
                {
                    isFirstPlayer = true;
                    if (ImTheFirst)
                    {
                        EnabledButton = true;
                    }
                    else
                    {
                        EnabledButton = false;
                    }
                }
                else
                {
                    isFirstPlayer = false;
                    if (ImTheFirst)
                    {
                        EnabledButton = false;
                    }
                    else
                    {
                        EnabledButton = true;
                    }
                }
            }
            else
            {
                isFirstPlayer = false;
                RecommendedFirstCheckout = Set_description(PointsFirstPlayer); 
                if (ImTheFirst)
                {
                    EnabledButton = false;
                }
                else
                {
                    EnabledButton = true;
                }
            }
        }


        private void SetSecondPlayersPoints(string thrownpoint)
        {
            if (int.Parse(thrownpoint) > int.Parse(PointsSecondPlayer) || int.Parse(PointsSecondPlayer) - int.Parse(thrownpoint) == 1)
            {
                thrownpoint = "0";
            }
            PointsSecondPlayer = (int.Parse(PointsSecondPlayer) - int.Parse(thrownpoint)).ToString();
            if (PointsSecondPlayer == "0")
            {
                if (int.Parse(thrownpoint) > MyHighestCheckOut)
                {
                    MyHighestCheckOut = int.Parse(thrownpoint);
                }

                var SumLegPoints = 0;
                for (int i = 0; i < MyLegScores.Count; i++)
                {
                    SumLegPoints += MyLegScores[i];
                }
                if (MyLegScores.Count == 3 && SumLegPoints == 501)
                {
                    MyNineDarters++;
                }
                AllWonLegsOnTheMatchSecond++;
                SecondPlayerWonLeg++;
                MyLegScores.Clear();
                SetDefaultValues();
                TextSpeach($"{SecondPlayerName} nyerte a(z) {allPlayedLegs}. leget!");
                if (allPlayedLegs % 2 == 0)
                {
                    isFirstPlayer = true;
                    if (ImTheFirst)
                    {
                        EnabledButton = true;
                    }
                    else
                    {
                        EnabledButton = false;
                    }
                }
                else
                {
                    isFirstPlayer = false;
                    if (ImTheFirst)
                    {
                        EnabledButton = false;
                    }
                    else
                    {
                        EnabledButton = true;
                    }
                }
            }
            else
            {
                RecommendedSecondCheckout = Set_description(PointsSecondPlayer);
                isFirstPlayer = true;
                if (ImTheFirst)
                {
                    EnabledButton = true;
                }
                else
                {
                    EnabledButton = false;
                }
            }
        }




        private async Task TextSpeach(string text)

        {
            var locales = await TextToSpeech.GetLocalesAsync();
            
            var hungarianLaungage = locales.FirstOrDefault(l => l.Language.Contains("hu"));
            var settings = new SpeechOptions()
            {
                Volume = 1.0f,
                Pitch = 1.0f,
                Locale = hungarianLaungage
            };
            
            await TextToSpeech.SpeakAsync(text, settings);
        }

        [RelayCommand]

        private async void SendPoints(string point) 
        {
            if (impossibleThrows.Contains(point) || int.Parse(point) > 180)
            {
                await Application.Current.MainPage.DisplayAlert("HIBA!", $"Nem lehetséges {point} pontot dobni!", "OK");
            }
            else
            {
                _signalRService?.PassPoints(MatchId, SecStoreItems.UserId, int.Parse(point));
                if (ImTheFirst)
                {
                    MyPlayerPoints += int.Parse(point);
                    MyLegScores.Add(int.Parse(point));
                    MyPlayerAllThrownDarts +=3;
                    if (point == "180")
                    {
                        MyPlayer180s++;
                    }
                    SetFirstPlayersPoints(point);
                    TextSpeach(point);
                    ShowCheckOutPopUpsFirstPlayer();
                    await CheckMatchWinner();
                }
                else
                {
                    MyPlayerPoints += int.Parse(point);
                    MyLegScores.Add(int.Parse(point));
                    MyPlayerAllThrownDarts +=3;
                    if (point == "180")
                    {
                        MyPlayer180s++;
                    }
                    SetSecondPlayersPoints(point);
                    TextSpeach(point);
                    ShowCheckOutPopUpsSecondPlayer();
                    await CheckMatchWinner();
                }
            }
            Points = "";

        }

        private async void SetDefaultValues()
        {
            PointsSecondPlayer = settings.StartingPoint.ToString();
            PointsFirstPlayer = settings.StartingPoint.ToString();
            RecommendedFirstCheckout = "";
            RecommendedSecondCheckout = "";
        }


        string description = "";
        private string Set_description(string point)
        {
            foreach (var pv in checkOutTable)
            {
                if (checkOutTable.ContainsKey(int.Parse(point)) && pv.Key == int.Parse(point))
                {
                    description = pv.Value;
                    break;
                }
                else
                {
                    description = "";
                }
            }
            return description;
        }


        private async Task CheckMatchWinner()
        {
            if (NeedToWinSets != 1)
            {
                if (FirstPlayerWonLeg == needToWinLegs)
                {
                    FirstPlayerSetsWon++;
                    TextSpeach($"{StartingPlayerName} nyerte a  {allPlayedSet}. szettet!");
                    SetDefaultValues();
                    AllPlayedLegsOnTheMatch += allPlayedLegs;
                    FirstPlayerWonLeg = SecondPlayerWonLeg = 0;
                }
                else if (SecondPlayerWonLeg == needToWinLegs)
                {
                    SecondPlayerSetsWon++;

                    TextSpeach($"{SecondPlayerName} nyerte a {allPlayedSet}. szettet!");
                    SetDefaultValues();
                    AllPlayedLegsOnTheMatch += allPlayedLegs;
                    FirstPlayerWonLeg = SecondPlayerWonLeg = 0;
                }
                if (FirstPlayerSetsWon == needToWinSets)
                {
                    TextSpeach($"{StartingPlayerName} nyerte a mérkőzést {FirstPlayerSetsWon}-{SecondPlayerSetsWon} arányban!");
                    if (ImTheFirst)
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                    }
                    else
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                    }
                    SetDefaultValues();
                    Thread.Sleep(15000);
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                else if(SecondPlayerSetsWon == needToWinSets){
                    TextSpeach($"{SecondPlayerName} nyerte a mérkőzést {SecondPlayerSetsWon}-{FirstPlayerSetsWon} arányban!");
                    if (ImTheFirst)
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                    }
                    else
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = AllPlayedLegsOnTheMatch, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                    }
                    SetDefaultValues();
                    Thread.Sleep(15000);
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
            }
            else
            {
                if (FirstPlayerWonLeg == needToWinLegs)
                {
                    TextSpeach($"{StartingPlayerName} nyerte a mérkőzést {FirstPlayerWonLeg}-{SecondPlayerWonLeg} arányban!");
                    if (ImTheFirst)
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                    }
                    else
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                    }
                    SetDefaultValues();
                    Thread.Sleep(15000);
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                else if (SecondPlayerWonLeg == needToWinLegs)
                {
                    TextSpeach($"{SecondPlayerName} nyerte a mérkőzést {SecondPlayerWonLeg}-{FirstPlayerWonLeg} arányban!");
                    if (ImTheFirst)
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchFirst, Sets = allPlayedSet, SetsWon = FirstPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = false });
                        }
                    }
                    else
                    {
                        if (IsFriendlyMatch)
                        {
                            await _signalRService.EndFriendlyMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                        else
                        {
                            await _signalRService.EndTournamentMatch(MatchId, SecStoreItems.UserId, new EndMatchModel { Legs = allPlayedLegs, LegsWon = AllWonLegsOnTheMatchSecond, Sets = allPlayedSet, SetsWon = SecondPlayerSetsWon, Averages = MyAverage, CheckoutPercentage = 45.56, HighestCheckout = MyHighestCheckOut, Max180s = MyPlayer180s, NineDarter = MyNineDarters, Won = true });
                        }
                    }
                    SetDefaultValues();
                    Thread.Sleep(15000);
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
            }

        }

        private void ShowCheckOutPopUpsFirstPlayer()
        {
            if(int.Parse(PointsFirstPlayer) <= 50 && int.Parse(PointsFirstPlayer) > 0)
            {
                SimpleCheckoutPopUp popUp = new SimpleCheckoutPopUp(new SimpleCheckOutPopUpViewModel());
                Application.Current.MainPage.ShowPopup(popUp);
            }
            if (int.Parse(PointsFirstPlayer) == 0)
            {
                EndGameCheckOutPopUp EndPopUp = new EndGameCheckOutPopUp();
                Application.Current.MainPage.ShowPopup(EndPopUp);
            }
        }

        private void ShowCheckOutPopUpsSecondPlayer()
        {
            if (int.Parse(PointsSecondPlayer) <= 50 && int.Parse(PointsSecondPlayer) > 0)
            {
                SimpleCheckoutPopUp popUp = new SimpleCheckoutPopUp(new SimpleCheckOutPopUpViewModel());
                Application.Current.MainPage.ShowPopup(popUp);
            }
            if (int.Parse(PointsSecondPlayer) == 0)
            {
                EndGameCheckOutPopUp EndPopUp = new EndGameCheckOutPopUp();
                Application.Current.MainPage.ShowPopup(EndPopUp);
            }
        }


        [RelayCommand]
        private void RemoveADigit() 
        {
            string original = Points;
            Points = original.Length > 0 ? original.Substring(0, original.Length - 1) : original;
        }
    }
}
