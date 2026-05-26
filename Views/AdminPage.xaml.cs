using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Views
{
    public partial class AdminPage : ContentPage
    {
        public AdminPage(ViewModels.AdminPageViewModel viewModel)
        {
            InitializeComponent();
            // שמירה על החלק הקריטי של הזרקת ה-ViewModel
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 1. שליפת המשתמש המחובר מה-App
            var currentUser = (App.Current as App)?.CurrentUser;

            // 2. בדיקה הרמטית: אם הוא לא אדמין, חוסמים וזורקים אותו ל-MainPage
            if (currentUser == null || !currentUser.IsAdmin)
            {
                // הצגת הודעת שגיאה קופצת
                await DisplayAlert("Access Denied", "You do not have administrative privileges to access this page.", "OK");

                // ניווט כפוי ומיידי חזרה לדאשבורד הרגיל
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}