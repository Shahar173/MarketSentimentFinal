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
    public class SignUpViewModel : INotifyPropertyChanged
    {
        #region Fields
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _isBusy;
        #endregion

        #region Properties
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand SignUpCommand { get; }
        public ICommand BackToLoginCommand { get; }
        #endregion

        #region Constructor
        public SignUpViewModel()
        {
            SignUpCommand = new Command(OnSignUp);
            // חזרה אחורה לדף הקודם (לוגין)
            BackToLoginCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }
        #endregion

        #region Methods
        private async void OnSignUp()
        {
            // בדיקה שכל השדות מלאים
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Missing Info", "Please fill in all fields", "OK");
                return;
            }

            // בדיקת אורך סיסמה
            if (Password.Length < 4)
            {
                await Shell.Current.DisplayAlert("Weak Password", "Password must be at least 4 characters", "OK");
                return;
            }

            IsBusy = true;
            await Task.Delay(2000);
            IsBusy = false;

            await Shell.Current.DisplayAlert("Success", $"Welcome {FirstName}!", "OK");
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