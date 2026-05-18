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

            Shell.SetBackgroundColor(this, Colors.Transparent);
            Shell.SetTitleColor(this, Colors.White);

            Microsoft.Maui.Handlers.ToolbarHandler.Mapper.AppendToMapping("CustomToolbar", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetContentInsetsAbsolute(0, 0);
                handler.PlatformView.ContentInsetStartWithNavigation = 0;
#endif
            });

            // Standard route registrations
            Routing.RegisterRoute("SignUpPage", typeof(SignUpPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));

            // Registered ONLY here so they act as standard stack pushes
            Routing.RegisterRoute("ViewNewsPage", typeof(ViewNewsPage));
            Routing.RegisterRoute("NewsDetailsPage", typeof(NewsDetailsPage));
            Routing.RegisterRoute("SentimentDetailsPage", typeof(SentimentDetailsPage));
            Routing.RegisterRoute("FearAndGreedPage", typeof(FearAndGreedPage));
        }
    }
}