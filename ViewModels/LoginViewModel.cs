using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;
using MarketSentimentFinal.Views;

namespace MarketSentimentFinal.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAppUserRepository _dbService;

        [ObservableProperty] private bool _isBusy;
        [ObservableProperty] private string _errorMessage;
        [ObservableProperty] private string _userEmail;
        [ObservableProperty] private string _userPassword;

        public LoginViewModel(IAppUserRepository dbService)
        {
            _dbService = dbService;

            // אתחול שדות
            UserEmail = string.Empty;
            UserPassword = string.Empty;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(UserPassword))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter credentials", "OK");
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // 1. קריאה ל-Firebase
                AppUser user = await _dbService.SignInAsync(UserEmail, UserPassword);

                if (user != null)
                {
                    // 2. שמירת המשתמש ב-App Class
                    if (App.Current is App mainApp)
                    {
                        mainApp.CurrentUser = user;
                    }

                    IsBusy = false;

                    // 3. החלפת ה-Root של האפליקציה ל-AppShell
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // שליפת ה-AppShell מה-Services (כך הוא מקבל את ה-ViewModel שלו אוטומטית)
                        var shell = IPlatformApplication.Current?.Services.GetService<AppShell>();

                        if (shell != null)
                        {
                            Application.Current.MainPage = shell;
                        }
                    });
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Login Failed", "Invalid credentials.", "OK");
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                ErrorMessage = ex.Message;
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task GoToSignUp()
        {
            try
            {
                // מכיוון שאנחנו בתוך NavigationPage (לפני ה-Shell), משתמשים בזה:
                var signUpPage = IPlatformApplication.Current?.Services.GetService<SignUpPage>();
                if (signUpPage != null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(signUpPage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation Error: {ex.Message}");
            }
        }
    }
}