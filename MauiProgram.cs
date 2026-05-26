using MarketSentimentFinal.Services;
using MarketSentimentFinal.Services.DBService;
using MarketSentimentFinal.ViewModels;
using MarketSentimentFinal.Views;
using MarketSentimentFinal.Views.News;
using MarketSentimentFinal.ViewModels.News;
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
            // רישום שירותים (Services) להזרקת תלויות (DI) - הופך את הלוגיקה לניתנת לשימוש חוזר בכל האפליקציה
            builder.Services.AddSingleton<IAppLogger, LogService>();
            builder.Services.AddSingleton<IAuthService, FirebaseAuthService>();
            builder.Services.AddSingleton<IAppUserRepository, FirebaseUsersRepository>();
            builder.Services.AddSingleton<INewsService, CryptoNewsService>();

            // --- רישום ה-Views ---
            // רישום דפי ה-UI (Views) - Transient ליצירה מחדש בכל כניסה, Singleton למסכים מרכזיים שנשארים בזיכרון
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignUpPage>();

            builder.Services.AddTransient<UserDetailsPage>();
            builder.Services.AddTransient<AdminPage>();

            builder.Services.AddTransient<UsersListPage>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<ViewNewsPage>();

            builder.Services.AddTransient<NewsDetailsPage>();
            builder.Services.AddTransient<SentimentDetailsPage>();

            builder.Services.AddTransient<FearAndGreedPage>();
            builder.Services.AddTransient<WhaleTrackerPage>();

            // --- רישום ה-ViewModels ---
            // רישום שכבת הלוגיקה (ViewModels) - מקשר בין הנתונים לבין ה-UI לפי עקרון ה-MVVM
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<UserDetailsPageViewModel>();
            builder.Services.AddTransient<AdminPageViewModel>();
            builder.Services.AddTransient<UsersListViewModel>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();

            // News ViewModels
            builder.Services.AddTransient<ViewNewsViewModel>();
            builder.Services.AddTransient<NewsDetailsViewModel>();

            builder.Services.AddTransient<SentimentDetailsViewModel>();
            builder.Services.AddTransient<FearAndGreedViewModel>();

            // תוקן: רישום ה-ViewModel של הלווייתנים
            builder.Services.AddTransient<WhaleTrackerViewModel>();

#if DEBUG
            // הפעלת לוגים (Logging) לצורכי ניפוי שגיאות רק במצב פיתוח (Debug)
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}