using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.ViewModels
{
    public class FearAndGreedViewModel : INotifyPropertyChanged
    {
        private int _currentIndexValue;
        private string _indexClassificationText;
        private string _explanationText;
        private Color _indexColorGlow;

        public event PropertyChangedEventHandler PropertyChanged;

        public FearAndGreedViewModel()
        {
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            // Set up our current base preview parameters loop mapping
            EvaluateFearGreedMatrix(62);
        }

        public ICommand GoBackCommand { get; }

        public int CurrentIndexValue
        {
            get => _currentIndexValue;
            set { _currentIndexValue = value; OnPropertyChanged(); }
        }

        public string IndexClassificationText
        {
            get => _indexClassificationText;
            set { _indexClassificationText = value; OnPropertyChanged(); }
        }

        public string ExplanationText
        {
            get => _explanationText;
            set { _explanationText = value; OnPropertyChanged(); }
        }

        public Color IndexColorGlow
        {
            get => _indexColorGlow;
            set { _indexColorGlow = value; OnPropertyChanged(); }
        }

        private void EvaluateFearGreedMatrix(int numericScore)
        {
            CurrentIndexValue = numericScore;

            // Strict parameter mapping thresholds processing tree
            if (numericScore <= 25)
            {
                IndexClassificationText = "Extreme Fear";
                IndexColorGlow = Color.FromArgb("#FF5252"); // Crimson red
                ExplanationText = "Market participant panic is driving deep asset deviations away from their baseline macro curves. High retail capital liquidation values present historical structural risk buffer support zones.";
            }
            else if (numericScore <= 45)
            {
                IndexClassificationText = "Fear";
                IndexColorGlow = Color.FromArgb("#FFAB40"); // Orange
                ExplanationText = "Capital inflows are running low due to widespread distribution anxieties. Volume metrics show patterns of strategic wallet accumulation by institutional buyers.";
            }
            else if (numericScore <= 55)
            {
                IndexClassificationText = "Neutral";
                IndexColorGlow = Color.FromArgb("#B0BEC5"); // Slate Gray
                ExplanationText = "Trading ranges remain bound with light volume distributions. Ecosystem triggers are holding steady while awaiting macro data trends.";
            }
            else if (numericScore <= 75)
            {
                IndexClassificationText = "Greed";
                IndexColorGlow = Color.FromArgb("#00E676"); // Vibrant Mint Green
                ExplanationText = "Buy wall structural expansion accelerates as volume scales over short-term resistances. Capital speed shows high momentum, with retail fear of missing out (FOMO) beginning to mount.";
            }
            else
            {
                IndexClassificationText = "Extreme Greed";
                IndexColorGlow = Color.FromArgb("#00E5FF"); // Electric Teal Cyan
                ExplanationText = "Speculative leverage metrics have climbed into critical overbought quadrants. Distribution tracking suggests elevated risks of trailing stop cascades and sharp volatility pullbacks.";
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}