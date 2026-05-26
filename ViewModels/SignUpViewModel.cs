using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;
using MarketSentimentFinal.Services.DBService;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

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

            // נתוני התחלה ריקים
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

                // הודעת הצלחה למשתמש
                await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {FirstName}!", "OK");

                // תוקן: החלפת ה-Root של האפליקציה ל-AppShell בצורה בטוחה ב-Main Thread
                // זה מונע את ה-NullReferenceException מאחר וה-Shell עוד לא קיים בשלב זה
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var shell = IPlatformApplication.Current?.Services.GetService<AppShell>();

                    if (shell != null)
                    {
                        Application.Current.MainPage = shell;
                    }
                });
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
            // מאחר ונכנסנו באמצעות Navigation.PushAsync, אנחנו יוצאים בצורה בטוחה עם PopAsync
            // זה עוקף את ה-Shell שעדיין לא קיים בשלב זה, ומונע לחלוטין את הקריסה!
            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
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