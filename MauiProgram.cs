using Microsoft.Extensions.Logging;
using MarketSentimentFinal.Views;
using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // רישום הדפים וה-ViewModels לטובת ה-Binding והניווט
            // Views
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<MainPage>();

            // ViewModels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}