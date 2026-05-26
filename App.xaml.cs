using MarketSentimentFinal.Models;
using MarketSentimentFinal.Views;
using MarketSentimentFinal.ViewModels; // הוספנו כדי לגשת ל-ViewModel
using System.Threading.Tasks; // הוספנו כדי להשתמש ב-Task

namespace MarketSentimentFinal
{
    public partial class App : Application
    {
        public AppUser? CurrentUser { get; set; } = null;

        public App()
        {
            InitializeComponent();

            // טעינה מקדימה של נתוני הלווייתנים ברקע (Fire-and-Forget)
            _ = Task.Run(async () =>
            {
                var vm = new WhaleTrackerViewModel();
                await vm.InitializeAsync();
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var loginPage = IPlatformApplication.Current!.Services.GetService<LoginPage>()!;
            return new Window(new NavigationPage(loginPage));
        }
    }
}