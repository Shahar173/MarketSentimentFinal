using System;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MarketSentimentFinal.Models;
using MarketSentimentFinal; // Added to resolve your root App reference cleanly

namespace MarketSentimentFinal.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        // Core Interactive Panel Interface Action Pointers
        public ICommand GoToHomeCommand { get; }
        public ICommand GoToAccountCommand { get; }
        public ICommand GoToAdminCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand GoToNewsCommand { get; }
        public ICommand GoToSentimentDetailsCommand { get; }

        // Added the property definition for your third button
        public ICommand GoToFearAndGreedCommand { get; }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set { _isAdmin = value; OnPropertyChanged(); }
        }

        public MainPageViewModel()
        {
            // Shell System Routing Interceptor Mappings
            GoToHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
            GoToAccountCommand = new Command(async () => await Shell.Current.GoToAsync("UserDetailsPage"));
            GoToAdminCommand = new Command(async () => await Shell.Current.GoToAsync("AdminPage"));
            LogoutCommand = new Command(OnLogout);

            // Interface Vector Redirect Pipeline Handlers
            GoToNewsCommand = new Command(async () => await Shell.Current.GoToAsync("ViewNewsPage"));
            GoToSentimentDetailsCommand = new Command(async () => await Shell.Current.GoToAsync("SentimentDetailsPage"));

            // Added the initialization routing pointer for the Fear and Greed page
            GoToFearAndGreedCommand = new Command(async () => await Shell.Current.GoToAsync("FearAndGreedPage"));

            // Core Global State Context Security Flags Evaluation
            IsAdmin = (App.Current as App)?.CurrentUser?.IsAdmin ?? false;
        }

        private async void OnLogout()
        {
            bool answer = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (answer)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}