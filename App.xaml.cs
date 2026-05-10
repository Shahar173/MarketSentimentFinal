using MarketSentimentFinal.Models;
using MarketSentimentFinal.Views;

namespace MarketSentimentFinal
{
    public partial class App : Application
    {
        // המשתנה הקריטי לשמירת המשתמש
        public AppUser? CurrentUser { get; set; } = null;

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // אנחנו שולפים את ה-LoginPage מהשירותים
            var loginPage = IPlatformApplication.Current!.Services.GetService<LoginPage>()!;

            // מחזירים חלון חדש שהדף שלו הוא LoginPage עטוף ב-NavigationPage
            // זה מה שגורם לאפליקציה להתחיל בלוגין ולא במסך הראשי
            return new Window(new NavigationPage(loginPage));
        }
    }
}