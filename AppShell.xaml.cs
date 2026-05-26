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
            BindingContext = viewModel;

            // Route registrations
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ViewNewsPage), typeof(ViewNewsPage));
            Routing.RegisterRoute(nameof(NewsDetailsPage), typeof(NewsDetailsPage));
            Routing.RegisterRoute(nameof(SentimentDetailsPage), typeof(SentimentDetailsPage));
            Routing.RegisterRoute(nameof(FearAndGreedPage), typeof(FearAndGreedPage));
            Routing.RegisterRoute(nameof(UserDetailsPage), typeof(UserDetailsPage));

            // שוחרר מחסימה: רישום הראוטים עבור הניווט מהדאשבורד ומהתפריט הצף
            Routing.RegisterRoute("WhaleTrackerPage", typeof(WhaleTrackerPage));

            // תוקן: רישום הראוט עבור עמוד פרטי ההעברה של הלווייתנים
            Routing.RegisterRoute("TransactionDetailsPage", typeof(TransactionDetailsPage));

            //Routing.RegisterRoute("ChatAssistantPage", typeof(ChatAssistantPage));
        }
    }
}