using System.Windows.Input;
using MarketSentimentFinal.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MarketSentimentFinal.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        // המאפיין IsAdmin מושך את הנתונים מהמשתמש הנוכחי ב-App
        public bool IsAdmin => (App.Current as App)?.CurrentUser?.IsAdmin ?? false;

        public ICommand GoToHomeCommand { get; }
        public ICommand GoToAccountCommand { get; }
        public ICommand GoToAdminCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand GoBackCommand { get; }

        public AppShellViewModel()
        {
            // שימוש ב-// מבטיח ניווט אבסולוטי בתוך ה-Shell
            GoToHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
            GoToAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//UserDetailsPage"));
            GoToAdminCommand = new Command(async () => await Shell.Current.GoToAsync("//AdminPage"));
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            LogoutCommand = new Command(async () => await Logout());
        }

        public void NotifyIsAdminChanged()
        {
            OnPropertyChanged(nameof(IsAdmin));
        }

        private async Task Logout()
        {
            // 1. איפוס המשתמש
            if (App.Current is App app) app.CurrentUser = null;

            NotifyIsAdminChanged();

            // 2. חזרה ללוגין על ידי החלפת ה-MainPage (הדרך הבטוחה ביותר אצלך)
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var loginPage = IPlatformApplication.Current?.Services.GetService<LoginPage>();
                if (loginPage != null)
                {
                    // אנחנו עוטפים ב-NavigationPage כדי שהניווט ל-SignUp ימשיך לעבוד
                    Application.Current.MainPage = new NavigationPage(loginPage);
                }
            });
        }
    }
}