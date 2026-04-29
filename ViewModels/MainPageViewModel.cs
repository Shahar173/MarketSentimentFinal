using MarketSentimentFinal.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<NewsArticle> NewsItems { get; set; } = new();

        // פקודות ניווט וחדשות
        public ICommand RefreshNewsCommand { get; }
        public ICommand GoToHomeCommand { get; }
        public ICommand GoToAccountCommand { get; }
        public ICommand GoToAdminCommand { get; }
        public ICommand LogoutCommand { get; }

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

        private bool _isAdmin;
        public bool IsAdmin { get => _isAdmin; set { _isAdmin = value; OnPropertyChanged(); } }

        public MainPageViewModel()
        {
            // אתחול פקודות
            RefreshNewsCommand = new Command(async () => await LoadNews());
            GoToHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
            GoToAccountCommand = new Command(async () => await Shell.Current.GoToAsync("UserDetailsPage"));
            GoToAdminCommand = new Command(async () => await Shell.Current.GoToAsync("AdminPage"));
            LogoutCommand = new Command(OnLogout);

            // בדיקת הרשאות אדמין מה-App
            IsAdmin = (App.Current as App)?.CurrentUser?.IsAdmin ?? false;

            _ = LoadNews();
        }

        private async Task LoadNews()
        {
            IsBusy = true;
            await Task.Delay(800);

            NewsItems.Clear();
            NewsItems.Add(new NewsArticle
            {
                Title = "Bitcoin Hits New Resistance at $72K",
                Description = "Analysts suggest a bullish breakout if BTC holds the $70,000 support level through the weekend.",
                Source = "CryptoNews",
                Sentiment = "BULLISH",
                SentimentColor = Color.FromArgb("#2ECC71"),
                PublishedAt = "2 mins ago"
            });

            NewsItems.Add(new NewsArticle
            {
                Title = "Gold (XAU/USD) Consolidation Continues",
                Description = "Market volatility remains low as traders await FOMC minutes. Major support found at $2,320.",
                Source = "ForexLive",
                Sentiment = "NEUTRAL",
                SentimentColor = Color.FromArgb("#95A5A6"),
                PublishedAt = "15 mins ago"
            });

            NewsItems.Add(new NewsArticle
            {
                Title = "Solana Network Hits Record Volume",
                Description = "DEX volume on Solana surpasses Ethereum for the third consecutive day driven by meme coin frenzy.",
                Source = "CryptoPanic",
                Sentiment = "BULLISH",
                SentimentColor = Color.FromArgb("#2ECC71"),
                PublishedAt = "1 hour ago"
            });

            IsBusy = false;
        }

        private async void OnLogout()
        {
            bool answer = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (answer)
            {
                // כאן תוכל להוסיף קריאה ל-AuthService לניתוק מה-Firebase
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}