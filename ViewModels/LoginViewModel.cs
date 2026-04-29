using MarketSentimentFinal.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Fields
        private IAppUserRepository _db;
        private string _userEmail = string.Empty;
        private string _userPassword = string.Empty;
        private bool _isBusy;
        #endregion

        #region Properties
        public string UserEmail
        {
            get => _userEmail;
            set { _userEmail = value; OnPropertyChanged(); }
        }

        public string UserPassword
        {
            get => _userPassword;
            set { _userPassword = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand GoToSignUpCommand { get; }
        #endregion

        #region Constructor
        public LoginViewModel(IAppUserRepository dbService)
        {
            _db = dbService;
            LoginCommand = new Command(OnLogin);

            // Navigate to Sign Up Page
            GoToSignUpCommand = new Command(async () =>
                await Shell.Current.GoToAsync("SignUpPage"));
        }
        #endregion

        #region Methods
        private async void OnLogin()
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(UserPassword))
            {
                await Shell.Current.DisplayAlert("Login Error", "Please enter both email and password.", "OK");
                return;
            }
            else
            {
                try
                {
                    var user = await _db.SignInAsync(UserEmail, UserPassword);
                    IsBusy = false;

                    // Navigate to Main Page
                    (App.Current as App)!.CurrentUser = user; // Set the current user in the App class

                    var mainPage = IPlatformApplication.Current!.Services.GetService<AppShell>()!; // Resolve MainPage from the service provider
                    Application.Current!.Windows[0].Page = mainPage; // Reset the MainPage to refresh the navigation stack
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    throw;
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged; // הוספנו סימן שאלה

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}