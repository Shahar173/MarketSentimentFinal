using MarketSentimentFinal.Models;
using MarketSentimentFinal.Views; // וודא שיש לך תיקיית Models עם מחלקת AppUser

namespace MarketSentimentFinal
{
    public partial class App : Application
    {
        private Page _page;
        // המשתנה הקריטי ששומר את נתוני המשתמש המחובר לכל אורך הריצה
        public AppUser? CurrentUser { get; set; }

        public App(LoginPage view)
        {
            InitializeComponent();

            _page= view;
            // הגדרת הדף הראשי של האפליקציה כ-AppShell
            //MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // החזרת החלון הראשי עם ה-AppShell שהגדרנו
            return new Window(_page);
        }
    }
}