using DartsMobilApp.API;
using DartsMobilApp.SecureStorageItems;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    public class TimerCountDown
    {
        private Timer _timer;
        private int _remainingSeconds = 15 * 60;

        public event Action OnCountdownReset;

        public void Start()
        {
            // Ha már fut, akkor ne indítsuk el újra
            if (_timer != null)
            {
                return;
            }


            _timer = new Timer(TimerCallback, null, 0, 1000);
        }

        private async void TimerCallback(object state)
        {
            _remainingSeconds--;

            if (_remainingSeconds % 60 == 0) // Minden percben ellenőrizzük
            {
                await CheckAndResetTimer();
            }

            if (_remainingSeconds <= 0) // Ha lejárt, állítsuk vissza
            {
                _remainingSeconds = 15*60;
            }
        }

        private async Task CheckAndResetTimer()
        {
            if (_remainingSeconds < 5*60)
            {
                var Model = new RefreshTokenModel()
                {
                    refreshToken = SecStoreItems.RToken
                };
                var JsonContent = JsonSerializer.Serialize(Model);
                var content = new StringContent(JsonContent, Encoding.UTF8, "application/json");
                var response = await DartsAPI.PostRefreshAndGetNewAccess(content, SecStoreItems.UserId);
                var newAccessToken = response.accessToken;
                await SecureStorage.SetAsync("Token", newAccessToken);


                _remainingSeconds = 15*60;

               
               
                OnCountdownReset?.Invoke();
            }
        }

        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
        }
    }
}
