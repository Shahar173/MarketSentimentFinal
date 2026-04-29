using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services; // כאן נמצא ה-Interface המעודכן
using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class UserDetailsPageViewModel : ViewModelBase, IQueryAttributable
    {
        private readonly IAppUserRepository _userRepo;
        private AppUser? _selectedUser;
        private string? _firstName;
        private string? _lastName;
        private string? _email;
        private string? _mobile;
        private bool _isDeleteVisible;

        public string? FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
        public string? LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string? Mobile { get => _mobile; set { _mobile = value; OnPropertyChanged(); } }
        public bool IsDeleteVisible { get => _isDeleteVisible; set { _isDeleteVisible = value; OnPropertyChanged(); } }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public UserDetailsPageViewModel(IAppUserRepository userRepo)
        {
            _userRepo = userRepo;
            UpdateCommand = new Command(async () => await OnUpdate());
            DeleteCommand = new Command(async () => await OnDelete());

            LoadCurrentUser();
        }

        private void LoadCurrentUser()
        {
            var currentUser = (App.Current as App)?.CurrentUser;
            if (currentUser != null)
            {
                SetUserData(currentUser);
                IsDeleteVisible = currentUser.IsAdmin;
            }
        }

        private void SetUserData(AppUser user)
        {
            _selectedUser = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Mobile = user.Mobile;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("selectedUser", out var obj) && obj is AppUser u)
            {
                SetUserData(u);
                IsDeleteVisible = (App.Current as App)?.CurrentUser?.IsAdmin == true;
            }
        }

        private async Task OnUpdate()
        {
            if (_selectedUser == null || _userRepo == null) return;

            _selectedUser.FirstName = FirstName ?? string.Empty;
            _selectedUser.LastName = LastName ?? string.Empty;
            _selectedUser.Mobile = Mobile ?? string.Empty;

            // קורא ל-UpdateUser שסידרנו ב-Interface
            await _userRepo.UpdateUser(_selectedUser);

            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Success", "Profile updated!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task OnDelete()
        {
            if (_selectedUser == null || _userRepo == null) return;

            bool confirmed = await Shell.Current.DisplayAlert("Delete", "Are you sure?", "Yes", "No");
            if (!confirmed) return;

            // קורא ל-RemoveUser שסידרנו ב-Interface
            await _userRepo.RemoveUser(_selectedUser);
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}