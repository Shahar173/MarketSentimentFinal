using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.ViewModels
{
    public class SentimentDetailsViewModel : INotifyPropertyChanged
    {
        private int _sentimentScore;
        private string _marketStateText;
        private string _marketTrendText;
        private string _explanationText;
        private Color _stateColorGlow;
        private Color _trendColorText;

        public event PropertyChangedEventHandler PropertyChanged;

        public SentimentDetailsViewModel()
        {
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            LoadDiagnosticMetrics(67, "Positive", "Improving");
        }

        public ICommand GoBackCommand { get; }

        public int SentimentScore
        {
            get => _sentimentScore;
            set { _sentimentScore = value; OnPropertyChanged(); }
        }

        public string MarketStateText
        {
            get => _marketStateText;
            set { _marketStateText = value; OnPropertyChanged(); }
        }

        public string MarketTrendText
        {
            get => _marketTrendText;
            set { _marketTrendText = value; OnPropertyChanged(); }
        }

        public string ExplanationText
        {
            get => _explanationText;
            set { _explanationText = value; OnPropertyChanged(); }
        }

        public Color StateColorGlow
        {
            get => _stateColorGlow;
            set { _stateColorGlow = value; OnPropertyChanged(); }
        }

        public Color TrendColorText
        {
            get => _trendColorText;
            set { _trendColorText = value; OnPropertyChanged(); }
        }

        private void LoadDiagnosticMetrics(int score, string state, string trend)
        {
            SentimentScore = score;
            MarketStateText = state.ToUpper();
            MarketTrendText = trend;

            if (state.Equals("Positive", StringComparison.OrdinalIgnoreCase))
            {
                StateColorGlow = Color.FromArgb("#00E676");
            }
            else if (state.Equals("Negative", StringComparison.OrdinalIgnoreCase))
            {
                StateColorGlow = Color.FromArgb("#FF5252");
            }
            else
            {
                StateColorGlow = Color.FromArgb("#B0BEC5");
            }

            if (trend.Equals("Improving", StringComparison.OrdinalIgnoreCase))
            {
                TrendColorText = Color.FromArgb("#00E676");
            }
            else if (trend.Equals("Declining", StringComparison.OrdinalIgnoreCase))
            {
                TrendColorText = Color.FromArgb("#FF5252");
            }
            else
            {
                TrendColorText = Color.FromArgb("#00B4DB");
            }

            ExplanationText = "Ecosystem metrics are pointing to a high-conviction momentum wave. A consolidated tracking rating shows strong buy wall support profiles cross-referenced across live data feeds.";
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}