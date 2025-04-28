using CommunityToolkit.Maui;
using DartsMobilApp.Classes;
using DartsMobilApp.Pages;
using DartsMobilApp.Service;
using DartsMobilApp.Services;
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
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeBrands");
                });

            builder.Services.AddSingleton<TimerCountDown>();
            builder.Services.AddSingleton<SignalRService>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();
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
