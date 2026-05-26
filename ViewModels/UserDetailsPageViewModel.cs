using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

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
        public ICommand LogoutCommand { get; } // פקודה חדשה עבור התנתקות

        public UserDetailsPageViewModel(IAppUserRepository userRepo)
        {
            _userRepo = userRepo;
            UpdateCommand = new Command(async () => await OnUpdate());
            DeleteCommand = new Command(async () => await OnDelete());
            LogoutCommand = new Command(async () => await OnLogout()); // קישור הפקודה

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

            await _userRepo.UpdateAsync(_selectedUser);

            var app = App.Current as App;
            if (app != null && app.CurrentUser != null && app.CurrentUser.Email == _selectedUser.Email)
            {
                app.CurrentUser.FirstName = _selectedUser.FirstName;
                app.CurrentUser.LastName = _selectedUser.LastName;
                app.CurrentUser.Mobile = _selectedUser.Mobile;
            }

            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Success", "Profile updated!", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task OnDelete()
        {
            if (_selectedUser == null || _userRepo == null) return;

            bool confirmed = await Shell.Current.DisplayAlert("Delete Account",
                $"Are you sure you want to delete {FirstName}?", "Yes", "No");

            if (!confirmed) return;

            try
            {
                await _userRepo.DeleteAsync(_selectedUser);
                await Shell.Current.DisplayAlert("Deleted", "User has been removed.", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Could not delete user. Try again.", "OK");
            }
        }

        private async Task OnLogout()
        {
            // בקשת אישור יציאה מהמשתמש
            bool confirmed = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (!confirmed) return;

            // 1. ניקוי אובייקט המשתמש השמור בזיכרון האפליקציה
            if (App.Current is App mainApp)
            {
                mainApp.CurrentUser = null;
            }

            // 2. החזרת דף הבית למסך ה-LoginPage המקורי (שבירת ה-Shell הנוכחי למען בטיחות)
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var loginPage = IPlatformApplication.Current?.Services.GetService<Views.LoginPage>();
                if (loginPage != null)
                {
                    // אנחנו שמים את ה-LoginPage בתוך NavigationPage כדי שיוכל לעבור בהמשך ל-SignUp
                    Application.Current.MainPage = new NavigationPage(loginPage);
                }
            });
        }
    }
}