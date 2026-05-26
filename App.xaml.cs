using MarketSentimentFinal.Models;
using MarketSentimentFinal.Views;
using MarketSentimentFinal.ViewModels;
using System.Threading.Tasks;

namespace MarketSentimentFinal
{
    public partial class App : Application
    {
        // קבוצה 1: ניהול מצב האפליקציה (שמירת המשתמש המחובר כרגע)
        public AppUser? CurrentUser { get; set; } = null;

        public App()
        {
            InitializeComponent();

            // קבוצה 2: טעינה מקדימה (Background Initialization)
            // הרצת אתחול ה-ViewModel ברקע כדי לקצר זמני המתנה של המשתמש בעליית האפליקציה
            _ = Task.Run(async () =>
            {
                var vm = new WhaleTrackerViewModel();
                await vm.InitializeAsync();
            });
        }

        // קבוצה 3: הגדרת דף הכניסה הראשוני
        protected override Window CreateWindow(IActivationState? activationState)
        {
            // הגדרת דף ה-Login כדף הראשון שמוצג למשתמש בעת פתיחת האפליקציה
            var loginPage = IPlatformApplication.Current!.Services.GetService<LoginPage>()!;
            return new Window(new NavigationPage(loginPage));
        }
    }
}