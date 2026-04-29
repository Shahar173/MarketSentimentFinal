using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string _welcomeText = string.Empty;

        public string WelcomeText
        {
            get => _welcomeText;
            set { _welcomeText = value; OnPropertyChanged(); }
        }

        public ICommand GoToAccountCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainPageViewModel()
        {
            // שליפת המשתמש מה-App.xaml.cs שסידרנו קודם
            var user = (App.Current as App)?.CurrentUser;

            // לוגיקת הברכה מהפרויקט הישן
            WelcomeText = user != null
                ? $"Hello {user.FirstName} {user.LastName}"
                : "Welcome!";

            // פקודה למעבר לדף פרטי חשבון
            GoToAccountCommand = new Command(async () =>
                await Shell.Current.GoToAsync("//UserDetailsPage"));

            // פקודת התנתקות (Logout)
            LogoutCommand = new Command(async () =>
            {
                if (App.Current is App app)
                {
                    app.CurrentUser = null;
                }

                // חזרה לדף הלוגין וניקוי היסטוריית הניווט
                await Shell.Current.GoToAsync("//LoginPage");
            });
        }
    }
}