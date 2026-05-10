using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;
using MarketSentimentFinal.Services.DBService;

namespace MarketSentimentFinal.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IAppUserRepository _dbService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _firstName;

        [ObservableProperty] private string _lastName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _email;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _password;

        [ObservableProperty] private string _mobile;
        [ObservableProperty] private bool _isBusy;
        [ObservableProperty] private string _errorMessage;
        [ObservableProperty] private bool _signUpMessageVisible;

        public SignUpViewModel(IAppUserRepository dbService)
        {
            _dbService = dbService;

            // נתוני התחלה ריקים (כי ביקשת לנקות)
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Mobile = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(Validate))]
        private async Task SignUp()
        {
            IsBusy = true;
            SignUpMessageVisible = false;

            var newUser = new AppUser()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Mobile = Mobile ?? "0000000000",
                RegDate = DateTime.Now,
                LastLogin = DateTime.Now
            };

            try
            {
                newUser.Id = await _dbService.CreateAsync(newUser);

                if (App.Current is App mainApp)
                {
                    mainApp.CurrentUser = newUser;
                }

                IsBusy = false;
                await Shell.Current.DisplayAlert("Success", $"Welcome {FirstName}!", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                IsBusy = false;
                SignUpMessageVisible = true;
                ErrorMessage = ex.Message;
            }
        }

        [RelayCommand]
        private async Task BackToLogin()
        {
            // ניווט חזרה ללוגין בצורה מפורשת כדי למנוע תקיעה
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private bool Validate()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   Password.Length >= 6;
        }
    }
}