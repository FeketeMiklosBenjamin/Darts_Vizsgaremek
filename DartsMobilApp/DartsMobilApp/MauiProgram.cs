using CommunityToolkit.Maui;
using DartsMobilApp.Pages;
using DartsMobilApp.ViewModel;
using Microsoft.Extensions.Logging;

namespace DartsMobilApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<CompetitionsPage>();
            builder.Services.AddSingleton<CompetitionsViewModel>();
            builder.Services.AddSingleton<CounterPage>();
            builder.Services.AddSingleton<CounterViewModel>();
            builder.Services.AddSingleton<StatisticPage>();
            builder.Services.AddSingleton<StatisticViewModel>();
            builder.Services.AddSingleton<FriendlySetupPage>();
            builder.Services.AddSingleton<FriendlySetupViewModel>();
            builder.Services.AddSingleton<FriendlyMatchPage>();
            builder.Services.AddSingleton<FriendlyMatchViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
