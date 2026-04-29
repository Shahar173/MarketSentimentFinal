using MarketSentimentFinal.Models; // וודא שיש לך תיקיית Models עם מחלקת AppUser

namespace MarketSentimentFinal
{
    public partial class App : Application
    {
        // המשתנה הקריטי ששומר את נתוני המשתמש המחובר לכל אורך הריצה
        public AppUser? CurrentUser { get; set; }

        public App()
        {
            InitializeComponent();

            // הגדרת הדף הראשי של האפליקציה כ-AppShell
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // החזרת החלון הראשי עם ה-AppShell שהגדרנו
            return new Window(MainPage!);
        }
    }
}