using DartsMobilApp.API;
using DartsMobilApp.SecureStorageItems;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    public class TimerCountDown
    {
        private Timer _timer;
        private int _remainingSeconds = 5 * 60;

        public event Action OnCountdownReset;

        public void Start()
        {
            // Ha már fut, akkor ne indítsuk el újra
            if (_timer != null)
            {
                Debug.WriteLine("Timer már fut!");
                return;
            }

            Debug.WriteLine("Timer elindult!");

            _timer = new Timer(TimerCallback, null, 0, 1000);
        }

        private async void TimerCallback(object state)
        {
            _remainingSeconds--;
            Debug.WriteLine($"Timer: {_remainingSeconds} másodperc van hátra.");

            if (_remainingSeconds % 60 == 0) // Minden percben ellenőrizzük
            {
                await CheckAndResetTimer();
            }

            if (_remainingSeconds <= 0) // Ha lejárt, állítsuk vissza
            {
                _remainingSeconds = 5 * 60;
                Debug.WriteLine("Timer újraindítva!");
            }
        }

        private async Task CheckAndResetTimer()
        {
            if (_remainingSeconds < 5 * 60)
            {
                var content = new StringContent(SecStoreItems.RToken, Encoding.UTF8, "application/json");
                var newAccessToken =  DartsAPI.PostRefreshAndGetNewAccess(content, SecStoreItems.UserId).Result.ToString();
                await SecureStorage.SetAsync("Token", newAccessToken);

                _remainingSeconds = 5 * 60;
                Debug.WriteLine("Új token beállítva, timer újraindítva!");

                OnCountdownReset?.Invoke();
            }
        }

        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
            Debug.WriteLine("Timer leállítva!");
        }
    }
}
