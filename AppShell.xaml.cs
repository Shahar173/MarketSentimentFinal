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
            
            // Register these as strings to match your FloatingNavBar GoToAsync calls
            //Routing.RegisterRoute("WhaleTrackerPage", typeof(WhaleTrackerPage));
            //Routing.RegisterRoute("ChatAssistantPage", typeof(ChatAssistantPage));
        }
    }
}