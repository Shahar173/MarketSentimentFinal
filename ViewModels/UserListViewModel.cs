using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class UsersListViewModel : ViewModelBase
    {
        private readonly IAppUserRepository _userRepo;
        private string? _searchText;
        private List<AppUser> _allUsersList = new();

        public string? SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); OnSearch(); }
        }

        public ObservableCollection<AppUser> AllUsers { get; set; } = new();
        public ICommand SearchCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public ICommand UserDetailsPageCommand { get; }

        public UsersListViewModel(IAppUserRepository userRepo)
        {
            _userRepo = userRepo;
            SearchCommand = new Command(OnSearch);
            ClearFilterCommand = new Command(ClearFilter);
            UserDetailsPageCommand = new Command<AppUser>(GoToAccountPage);
            _ = LoadAllUsers();
        }

        public async Task LoadAllUsers()
        {
            var usersFromDb = _userRepo.GetAllAsync();
            _allUsersList = usersFromDb;
            UpdateCollection(usersFromDb);
        }

        private void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) { UpdateCollection(_allUsersList); return; }

            var filtered = _allUsersList.Where(u =>
                (u.FirstName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                (u.LastName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                (u.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true)
            ).ToList();

            UpdateCollection(filtered);
        }

        private void UpdateCollection(List<AppUser> users)
        {
            AllUsers.Clear();
            foreach (var user in users) AllUsers.Add(user);
        }

        private void ClearFilter() { SearchText = string.Empty; _ = LoadAllUsers(); }

        private async void GoToAccountPage(AppUser user)
        {
            if (user == null) return;
            var param = new Dictionary<string, object> { { "selectedUser", user } };
            await Shell.Current.GoToAsync("UserDetailsPage", param);
        }
    }
}