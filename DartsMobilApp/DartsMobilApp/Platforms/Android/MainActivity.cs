using Android.App;
using Android.Content.PM;
using DartsMobilApp.Classes;
using DartsMobilApp.SecureStorageItems;

namespace DartsMobilApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
  
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnDestroy()

        {
            if (SecStoreItems.IsChecked == "1")
            {
                LogOut.LogOutFunction();
                SecureStorage.Remove("Token");
                SecureStorage.Remove("RefreshToken");
                SecureStorage.Remove("UserName");
            }
            else
            {
                LogOut.LogOutFunction();
                SecureStorage.RemoveAll();
            }

        }
    }
}
