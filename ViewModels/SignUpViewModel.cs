using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;
using MarketSentimentFinal.Services.DBService;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

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

        // תוקן: הכפתור יגיב ויבדוק את עצמו מחדש בכל הקלדה של הטלפון
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        private string _mobile;

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
                Mobile = Mobile,
                RegDate = DateTime.Now,
                LastLogin = DateTime.Now,
                IsAdmin = false // ברירת מחדל למשתמש חדש
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

                // תוקן: הגדרת ה-Shell והעברה ישירה ומיידית למסך הראשי
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var shell = IPlatformApplication.Current?.Services.GetService<AppShell>();

                    if (shell != null)
                    {
                        Application.Current.MainPage = shell;

                        // ניתוב מוחלט שמנקה את ה-Stack ומכניס את המשתמש החדש ישר לתוך ה-Dashboard
                        await shell.GoToAsync("//MainPage");
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
            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        /// <summary>
        /// פונקציית ולידציה מורחבת לבדיקת שדות החובה ותקינות מספר הטלפון
        /// </summary>
        private bool Validate()
        {
            // 1. בדיקת שדות חובה בסיסיים
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }

            // 2. בדיקת אורך סיסמה
            if (Password.Length < 6)
            {
                return false;
            }

            // 3. בדיקה מספרית ואורך עבור מספר הטלפון
            if (string.IsNullOrWhiteSpace(Mobile))
            {
                return false;
            }

            // ניקוי תווים נפוצים כמו מקפים או רווחים במידה והמשתמש הקליד אותם (למשל 050-1234567)
            string cleanedMobile = Mobile.Replace("-", "").Replace(" ", "").CustomTrim();

            // וידוא שהקלט מכיל ספרות בלבד ושהאורך נע בין 9 ל-15 תווים (מתאים לפורמט מקומי ובינלאומי)
            bool isNumeric = cleanedMobile.All(char.IsDigit);
            bool isValidLength = cleanedMobile.Length >= 9 && cleanedMobile.Length <= 15;

            return isNumeric && isValidLength;
        }
    }

    // מחלקת עזר פנימית לטיפול בטוח במחרוזות
    public static class StringExtensions
    {
        public static string CustomTrim(ValueType? value) => value?.ToString()?.Trim() ?? string.Empty;
        public static string CustomTrim(this string value) => value?.Trim() ?? string.Empty;
    }
}