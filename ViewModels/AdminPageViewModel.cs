using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class AdminPageViewModel : ViewModelBase
    {
        public ICommand ViewUsersCommand { get; }

        public AdminPageViewModel()
        {
            ViewUsersCommand = new Command(async () =>
    await Shell.Current.GoToAsync("//UsersListPage"));
        }
    }
}