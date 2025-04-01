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
        private int _remainingSeconds = 250;

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
                _remainingSeconds = 250;
            }
        }

        private async Task CheckAndResetTimer()
        {
            if (_remainingSeconds < 250)
            {
                var Model = new RefreshTokenModel()
                {
                    refreshToken = SecStoreItems.RToken
                };
                var JsonContent = JsonSerializer.Serialize(Model);
                var content = new StringContent(JsonContent, Encoding.UTF8, "application/json");
                var newAccessToken =  DartsAPI.PostRefreshAndGetNewAccess(content, SecStoreItems.UserId).accessToken;
                await SecureStorage.SetAsync("Token", newAccessToken);


                _remainingSeconds = 250;

               
               
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
