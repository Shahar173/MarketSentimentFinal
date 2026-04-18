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
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);

            // Navigate to Sign Up Page
            GoToSignUpCommand = new Command(async () =>
                await Shell.Current.GoToAsync("SignUpPage"));
        }
        #endregion

        #region Methods
        private async void OnLogin()
        {
            if (string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(UserPassword))
            {
                await Shell.Current.DisplayAlert("Login Error", "Please enter both email and password.", "OK");
                return;
            }

            IsBusy = true;
            await Task.Delay(1500);
            IsBusy = false;

            await Shell.Current.GoToAsync("//MainPage");
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