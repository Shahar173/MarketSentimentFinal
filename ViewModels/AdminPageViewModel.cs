using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class AdminPageViewModel : ViewModelBase
    {
        public ICommand ViewUsersCommand { get; }

        public AdminPageViewModel()
        {
            // הניווט כאן מותאם ל-Route של רשימת המשתמשים
            ViewUsersCommand = new Command(async () =>
                await Shell.Current.GoToAsync("//UsersListPage"));
        }
    }
}