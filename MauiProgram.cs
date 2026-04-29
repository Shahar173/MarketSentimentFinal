using MarketSentimentFinal.Services;
using MarketSentimentFinal.ViewModels;
using MarketSentimentFinal.Views;
using MarketSentimentFinal.Services.DBService;
using Microsoft.Extensions.Logging;

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
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

            // --- רישום ה-Services ---
            builder.Services.AddSingleton<IAuthService, FirebaseAuthService>();
            builder.Services.AddSingleton<IAppUserRepository, FirebaseUsersRepository>();

            // --- רישום ה-Views ---
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<UserDetailsPage>(); // הוספתי את זה
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<AppShell>();

            // --- רישום ה-ViewModels ---
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<UserDetailsPageViewModel>(); // הוספתי את זה
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}