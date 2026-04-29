using System.Windows.Input;
using MarketSentimentFinal.Views;

namespace MarketSentimentFinal.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        // בדיקה האם המשתמש הוא אדמין כדי להציג כפתורי ניהול
        public bool IsAdmin => (App.Current as App)?.CurrentUser?.IsAdmin ?? false;

        public ICommand GoToHomeCommand { get; }
        public ICommand GoToAccountCommand { get; }
        public ICommand GoToAdminCommand { get; }
        public ICommand LogoutCommand { get; }

        public AppShellViewModel()
        {
            // ניווט לדף הבית
            GoToHomeCommand = new Command(async () =>
                await Shell.Current.GoToAsync("//MainPage"));

            // ניווט לדף פרטי משתמש
            GoToAccountCommand = new Command(async () =>
                await Shell.Current.GoToAsync("//UserDetailsPage"));

            // ניווט לדף ניהול
            GoToAdminCommand = new Command(async () =>
                await Shell.Current.GoToAsync("//AdminPage"));

            // פקודת התנתקות
            LogoutCommand = new Command(async () => await Logout());
        }

        // פונקציה לעדכון ה-UI כשהסטטוס של האדמין משתנה
        public void NotifyIsAdminChanged()
        {
            OnPropertyChanged(nameof(IsAdmin));
        }

        private async Task Logout()
        {
            // 1. איפוס המשתמש הנוכחי בזיכרון
            if (App.Current is App app)
            {
                app.CurrentUser = null;
            }

            // 2. עדכון ה-UI
            NotifyIsAdminChanged();

            // 3. ניווט חזרה לדף הלוגין דרך ה-Shell (הדרך הנכונה ב-MAUI)
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}