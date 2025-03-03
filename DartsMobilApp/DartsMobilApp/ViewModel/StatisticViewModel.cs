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

namespace DartsMobilApp.ViewModel
{
    public partial class StatisticViewModel : ObservableObject
    {


        string? AToken = SecureStorage.GetAsync("Token")?.Result?.ToString();
        HttpClient httpClient = new();

        [ObservableProperty]

        public StatisticModel playerStatistic;

        [RelayCommand]

        private void Appearing()
        {
            GetStatistics();
        }


        private async Task<StatisticModel> GetStatistics()
        {
            {
                if (string.IsNullOrEmpty(AToken))
                {
                    throw new Exception("Access token is missing. Please log in.");
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);

                var url = "https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/friendlystat"; 

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    PlayerStatistic = JsonSerializer.Deserialize<StatisticModel>(json);

                    return PlayerStatistic;
                }
                else
                {
                    throw new Exception($"Error fetching data: {response.StatusCode}");
                }
            }
        }
    }
}
