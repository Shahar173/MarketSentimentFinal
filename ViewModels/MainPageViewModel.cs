using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MarketSentimentFinal.ViewModels.News; // חובה כדי להכיר את ViewNewsViewModel

namespace MarketSentimentFinal.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new();
        public event PropertyChangedEventHandler PropertyChanged;

        // Navigation Commands
        public ICommand GoToHomeCommand { get; }
        public ICommand GoToNewsCommand { get; }
        public ICommand GoToFearAndGreedCommand { get; }
        public ICommand GoToChatAssistantCommand { get; }
        public ICommand GoToWhaleTrackerCommand { get; }
        public ICommand GoToAccountCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ChatComingSoonCommand { get; }

        // Dashboard Data
        private string _moodScore = "50"; // ברירת מחדל ניטרלית
        public string MoodScore
        {
            get => _moodScore;
            set
            {
                _moodScore = value;
                UpdateKnobPosition();
                OnPropertyChanged();
            }
        }

        private double _indicatorPosition;
        public double IndicatorPosition
        {
            get => _indicatorPosition;
            set { _indicatorPosition = value; OnPropertyChanged(); }
        }

        private string _moodStatusText = "NEUTRAL SENTIMENT";
        public string MoodStatusText { get => _moodStatusText; set { _moodStatusText = value; OnPropertyChanged(); } }

        private string _whaleBuyPercent = "50% Buy";
        public string WhaleBuyPercent { get => _whaleBuyPercent; set { _whaleBuyPercent = value; OnPropertyChanged(); } }

        private string _whaleSellPercent = "50% Sell";
        public string WhaleSellPercent { get => _whaleSellPercent; set { _whaleSellPercent = value; OnPropertyChanged(); } }

        private string _fearGreedValue = "--";
        public string FearGreedValue { get => _fearGreedValue; set { _fearGreedValue = value; OnPropertyChanged(); } }

        private string _fearGreedStatus = "LOADING";
        public string FearGreedStatus { get => _fearGreedStatus; set { _fearGreedStatus = value; OnPropertyChanged(); } }

        private Color _fearGreedColor = Colors.White;
        public Color FearGreedColor { get => _fearGreedColor; set { _fearGreedColor = value; OnPropertyChanged(); } }

        public MainPageViewModel()
        {
            GoToHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
            GoToNewsCommand = new Command(async () => await Shell.Current.GoToAsync("ViewNewsPage"));
            GoToFearAndGreedCommand = new Command(async () => await Shell.Current.GoToAsync("FearAndGreedPage"));
            GoToChatAssistantCommand = new Command(async () => await Shell.Current.GoToAsync("ChatAssistantPage"));
            GoToWhaleTrackerCommand = new Command(async () => await Shell.Current.GoToAsync("WhaleTrackerPage"));
            GoToAccountCommand = new Command(async () => await Shell.Current.GoToAsync("UserDetailsPage"));
            LogoutCommand = new Command(OnLogout);
            ChatComingSoonCommand = new Command(async () =>
                await Shell.Current.DisplayAlert("AI Analyst", "Coming soon! We're working on it.", "OK"));

            // טעינה ראשונית של כל הנתונים הסטטיים מהזיכרון
            LoadDashboardData();
            _ = LoadFearAndGreedDataAsync();
        }

        // מתודה משולבת: מושכת חדשות לחלק העליון ולווייתנים לחלק התחתון
        public void LoadDashboardData()
        {
            // 1. קבלת ציון החדשות (Fundamental Sentiment) לחלק העליון של המסך
            int newsScore = ViewNewsViewModel.SharedNewsScore;
            MoodScore = newsScore.ToString();

            // עדכון המלל הראשי לפי ציון סנטימנט החדשות
            if (newsScore >= 65) MoodStatusText = "BULLISH OPTIMISM";
            else if (newsScore >= 45) MoodStatusText = "NEUTRAL SENTIMENT";
            else MoodStatusText = "BEARISH FEAR";

            // עדכון מיקום המחוג על פי הציון של החדשות
            UpdateKnobPosition();

            // 2. קבלת אחוזי הלווייתנים (On-Chain Data) לחלק התחתון של המסך
            WhaleBuyPercent = WhaleTrackerViewModel.SharedBuyPercent;
            WhaleSellPercent = WhaleTrackerViewModel.SharedSellPercent;
        }

        private void UpdateKnobPosition()
        {
            if (double.TryParse(MoodScore, out double score))
            {
                // 1. נגדיר את אורך הקו הריאלי שרואים במסך (בערך 295 פיקסלים)
                double totalWidth = 295;

                // 2. נוריד את קוטר העיגול (12 פיקסלים) כדי שכשהציון הוא 100 העיגול יעצר בדיוק בקצה הקו ולא יחרוג ממנו
                double maxTranslation = totalWidth - 12;

                // 3. נחשב את המיקום וננעל אותו בין 0 למקסימום החדש
                IndicatorPosition = Math.Clamp((score / 100.0) * maxTranslation, 0, maxTranslation);
            }
        }

        private async Task LoadFearAndGreedDataAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<MainPageAlternativeMeResult>("https://api.alternative.me/fng/?limit=1");
                if (response?.Data != null && response.Data.Count > 0)
                {
                    var latest = response.Data[0];
                    FearGreedValue = latest.Value;
                    if (int.TryParse(latest.Value, out int score)) EvaluateFearGreedColor(score);
                }
            }
            catch
            {
                FearGreedValue = "50";
                FearGreedStatus = "NEUTRAL";
                FearGreedColor = Color.FromArgb("#B0BEC5");
            }
        }

        private void EvaluateFearGreedColor(int numericScore)
        {
            if (numericScore <= 25) { FearGreedStatus = "EXTREME FEAR"; FearGreedColor = Color.FromArgb("#FF5252"); }
            else if (numericScore <= 45) { FearGreedStatus = "FEAR"; FearGreedColor = Color.FromArgb("#FFAB40"); }
            else if (numericScore <= 55) { FearGreedStatus = "NEUTRAL"; FearGreedColor = Color.FromArgb("#B0BEC5"); }
            else if (numericScore <= 75) { FearGreedStatus = "GREED"; FearGreedColor = Color.FromArgb("#00E676"); }
            else { FearGreedStatus = "EXTREME GREED"; FearGreedColor = Color.FromArgb("#00E5FF"); }
        }

        private async void OnLogout()
        {
            bool answer = await Shell.Current.DisplayAlert("Logout", "Are you sure?", "Yes", "No");
            if (answer) await Shell.Current.GoToAsync("//LoginPage");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainPageAlternativeMeResult { [JsonPropertyName("data")] public List<MainPageFearGreedApiItem> Data { get; set; } }
    public class MainPageFearGreedApiItem { [JsonPropertyName("value")] public string Value { get; set; } }
}