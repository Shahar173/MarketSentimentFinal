using MarketSentimentFinal.Views;

namespace MarketSentimentFinal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            #region Routes Registration
            // רישום הנתיבים מאפשר לנווט לדפים האלה מכל מקום בקוד
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            #endregion
        }
    }
}