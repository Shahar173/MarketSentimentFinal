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
                // ניסיון לנווט לדף (ברגע שתבנה אותו, זה פשוט יעבוד)
                await Shell.Current.GoToAsync("ChatAssistantPage");
            }
            catch
            {
                // אם הדף עדיין לא קיים, המשתמש יקבל את ההתראה
                await Shell.Current.DisplayAlert("Coming Soon", "AI Chat Assistant page is not created yet.", "OK");
            }
        }
    }
}