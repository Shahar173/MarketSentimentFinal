using System;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Views.Components
{
    public partial class FloatingNavBar : ContentView
    {
        public static readonly BindableProperty ActivePageProperty =
            BindableProperty.Create(
                nameof(ActivePage),
                typeof(string),
                typeof(FloatingNavBar),
                "Home");

        public string ActivePage
        {
            get => (string)GetValue(ActivePageProperty);
            set => SetValue(ActivePageProperty, value);
        }

        public FloatingNavBar()
        {
            InitializeComponent();
        }

        // ברגע שהרכיב מתווסף למסך, נבדוק את הרשאות המשתמש הנוכחי
        protected override void OnParentSet()
        {
            base.OnParentSet();

            var currentUser = (App.Current as App)?.CurrentUser;
            if (currentUser != null)
            {
                // הטאב יופיע אך ורק אם המשתמש הוא אדמין מורשה ב-Firebase
                AdminTab.IsVisible = currentUser.IsAdmin;
            }
            else
            {
                AdminTab.IsVisible = false;
            }
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void OnNewsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ViewNewsPage");
        }

        private async void OnAccountClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserDetailsPage");
        }

        private async void OnAdminClicked(object sender, EventArgs e)
        {
            // מעבר חלק לעמוד ניהול המערכת של האדמינים
            await Shell.Current.GoToAsync("//AdminPage");
        }

        private async void OnWhaleTrackerClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("WhaleTrackerPage");
            }
            catch
            {
                await Shell.Current.DisplayAlert("Coming Soon", "Whale Tracker page is not created yet.", "OK");
            }
        }

        private async void OnChatClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("ChatAssistantPage");
            }
            catch
            {
                await Shell.Current.DisplayAlert("Coming Soon", "AI Chat Assistant page is not created yet.", "OK");
            }
        }
    }
}