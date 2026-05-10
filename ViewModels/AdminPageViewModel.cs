using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class AdminPageViewModel : ViewModelBase
    {
        public ICommand ViewUsersCommand { get; }

        public AdminPageViewModel()
        {
            // התיקון: ניווט יחסי (בלי //) כדי שה-Shell ימצא את ה-Route הרשום
            ViewUsersCommand = new Command(async () =>
                await Shell.Current.GoToAsync("UsersListPage"));
        }
    }
}