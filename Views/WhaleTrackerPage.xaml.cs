using Microsoft.Maui.Controls;
using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views
{
    public partial class WhaleTrackerPage : ContentPage
    {
        public WhaleTrackerPage()
        {
            InitializeComponent();

            // זה התיקון: תוודא שה-BindingContext מוגדר
            // אם הכל עובד, תשאיר את זה ככה.
            this.BindingContext = new WhaleTrackerViewModel();
        }


        // חשוב: תוסיף את זה כדי שה-API יטען נתונים בכל פעם שהמשתמש מגיע לדף
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is WhaleTrackerViewModel viewModel)
            {
                // קריאה לפונקציה כדי לרענן נתונים בכל פעם שהדף מופיע
                // (תשתמש ב-RefreshCommand אם הוא קיים ב-ViewModel)
                if (viewModel.RefreshCommand.CanExecute(null))
                {
                    viewModel.RefreshCommand.Execute(null);
                }
            }
        }


    }
}