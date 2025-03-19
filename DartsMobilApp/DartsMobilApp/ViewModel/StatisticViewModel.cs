using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DartsMobilApp.Classes;
using System.Text.Json;
using DartsMobilApp.API;
using DartsMobilApp.SecureStorageItems;

namespace DartsMobilApp.ViewModel
{
    public partial class StatisticViewModel : ObservableObject
    {


        public string? AccessToken = SecStoreItems.AToken;

        [ObservableProperty]

        public StatisticModel playerStatistic;

        [RelayCommand]

        private void Appearing()
        {
            LoadStatistics();
        }


        private  StatisticModel LoadStatistics()
        {
            {
                if (string.IsNullOrEmpty(AccessToken))
                {
                    throw new Exception("Hiányzó Access Token! Kérem jelentkezzen be!");

                }
                PlayerStatistic = DartsAPI.GetStatistic();
                if (PlayerStatistic != null)
                {
                    return PlayerStatistic;
                }

                else
                {
                    throw new Exception($"Valami hiba történt!");
                }
            }
        }
    }
}
