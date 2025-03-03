using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.ViewModel
{
    public partial class StatisticViewModel : ObservableObject
    {

        string? AToken = SecureStorage.GetAsync("Token")?.Result?.ToString();
        HttpClient httpClient = new();


        [RelayCommand]

        private void Appearing()
        {

        }


        private async StatisticModel GetStatistics()
        {
            using (var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app/api/users/friendlystat"))
            {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", AToken);

                await httpClient.SendAsync(requestMessage);
            }
        }
    }
}
