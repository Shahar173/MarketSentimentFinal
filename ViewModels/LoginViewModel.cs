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
        private string _userEmail = string.Empty;
        private string _userPassword = string.Empty;
        private bool _isBusy;

        public string UserEmail
        {
            get => _userEmail;
            set
            {
                _userEmail = value;
                OnPropertyChanged();
            }
        }

        public string UserPassword
        {
            get => _userPassword;
            set
            {
                _userPassword = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
        }

        private async void OnLogin()
        {
            // Validation
            if (string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(UserPassword))
            {
                await Shell.Current.DisplayAlert("Login Error", "Please enter both email and password.", "OK");
                return;
            }

            IsBusy = true;

            // Simulated authentication delay (Firebase logic will go here later)
            await Task.Delay(1500);

            IsBusy = false;

            // Navigation to the MainPage
            // Note: Make sure "MainPage" is registered in your AppShell.xaml
            await Shell.Current.GoToAsync("//MainPage");
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
