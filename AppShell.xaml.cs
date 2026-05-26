using MarketSentimentFinal.ViewModels;
using MarketSentimentFinal.Views;
using MarketSentimentFinal.Views.News;

namespace MarketSentimentFinal
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // חיבור ה-ViewModel ל-Shell לצורך לוגיקת תפריטים וניווט

            // קבוצה 1: רישום דפי האפליקציה ב-Routing (מאפשר ניווט אליהם מכל מקום ע"י שם)
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ViewNewsPage), typeof(ViewNewsPage));
            Routing.RegisterRoute(nameof(NewsDetailsPage), typeof(NewsDetailsPage));
            Routing.RegisterRoute(nameof(SentimentDetailsPage), typeof(SentimentDetailsPage));
            Routing.RegisterRoute(nameof(FearAndGreedPage), typeof(FearAndGreedPage));
            Routing.RegisterRoute(nameof(UserDetailsPage), typeof(UserDetailsPage));

            // קבוצה 2: רישום ידני של דפים נוספים (עבור ניווט דינמי מתוך התפריט הצף והדאשבורד)
            Routing.RegisterRoute("WhaleTrackerPage", typeof(WhaleTrackerPage));
            Routing.RegisterRoute("TransactionDetailsPage", typeof(TransactionDetailsPage));
        }
    }
}