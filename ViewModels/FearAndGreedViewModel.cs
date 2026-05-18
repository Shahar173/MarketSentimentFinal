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
using Microsoft.Maui.Graphics;               // נדרש עבור Point
using Microsoft.Maui.Controls.Shapes;        // נדרש עבור PointCollection

namespace MarketSentimentFinal.ViewModels
{
    public class FearAndGreedViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new();

        private int _currentIndexValue;
        private string _indexClassificationText;
        private string _explanationText;
        private Color _indexColorGlow;
        private PointCollection _graphPoints; // שונה מ-string ל-PointCollection
        private bool _isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoBackCommand { get; }
        public ICommand RefreshCommand { get; }

        public FearAndGreedViewModel()
        {
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            RefreshCommand = new Command(async () => await LoadApiDataAsync());

            // אתחול ערכים ראשוניים זמניים למניעת קריסות רינדור (Fallback מקומי)
            EvaluateFearGreedMatrix(50);
            GraphPoints = new PointCollection
            {
                new Point(0, 140),
                new Point(75, 140),
                new Point(150, 140),
                new Point(225, 140),
                new Point(300, 140)
            };

            _ = LoadApiDataAsync();
        }

        #region Properties
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

        public PointCollection GraphPoints // שונה מ-string ל-PointCollection
        {
            get => _graphPoints;
            set { _graphPoints = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }
        #endregion

        private async Task LoadApiDataAsync()
        {
            if (IsLoading) return;
            IsLoading = true;

            try
            {
                var response = await _httpClient.GetFromJsonAsync<AlternativeMeResult>("https://api.alternative.me/fng/?limit=5");

                if (response?.Data != null && response.Data.Count > 0)
                {
                    var latest = response.Data[0];
                    if (int.TryParse(latest.Value, out int score))
                    {
                        EvaluateFearGreedMatrix(score);
                    }

                    var historicalData = response.Data;
                    historicalData.Reverse();

                    var points = new PointCollection();
                    int xSpacing = 75;

                    for (int i = 0; i < historicalData.Count; i++)
                    {
                        if (int.TryParse(historicalData[i].Value, out int dayValue))
                        {
                            double x = i * xSpacing;
                            double y = 160 - (dayValue * 1.4);

                            points.Add(new Point(x, y));
                        }
                    }

                    GraphPoints = points;
                }
            }
            catch (Exception)
            {
                EvaluateFearGreedMatrix(50);
                GraphPoints = new PointCollection
                {
                    new Point(0, 140),
                    new Point(75, 120),
                    new Point(150, 130),
                    new Point(225, 90),
                    new Point(300, 100)
                };
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void EvaluateFearGreedMatrix(int numericScore)
        {
            CurrentIndexValue = numericScore;

            if (numericScore <= 25)
            {
                IndexClassificationText = "Extreme Fear";
                IndexColorGlow = Color.FromArgb("#FF5252");
                ExplanationText = "Market participant panic is driving deep asset deviations away from their baseline macro curves. API analysis detects severe capital liquidations.";
            }
            else if (numericScore <= 45)
            {
                IndexClassificationText = "Fear";
                IndexColorGlow = Color.FromArgb("#FFAB40");
                ExplanationText = "Capital inflows are running low due to widespread distribution anxieties. Volume metrics show patterns of strategic wallet accumulation.";
            }
            else if (numericScore <= 55)
            {
                IndexClassificationText = "Neutral";
                IndexColorGlow = Color.FromArgb("#B0BEC5");
                ExplanationText = "Trading ranges remain bound with light volume distributions. Ecosystem triggers are holding steady while awaiting macro data trends.";
            }
            else if (numericScore <= 75)
            {
                IndexClassificationText = "Greed";
                IndexColorGlow = Color.FromArgb("#00E676");
                ExplanationText = "Buy wall structural expansion accelerates as volume scales over short-term resistances. Capital speed shows high momentum, retail FOMO is mounting.";
            }
            else
            {
                IndexClassificationText = "Extreme Greed";
                IndexColorGlow = Color.FromArgb("#00E5FF");
                ExplanationText = "Speculative leverage metrics have climbed into critical overbought quadrants. Distribution tracking suggests elevated risks of a sharp pullback.";
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #region API Data Transfer Objects (DTO)
    public class AlternativeMeResult
    {
        [JsonPropertyName("data")]
        public List<FearGreedApiItem> Data { get; set; }
    }

    public class FearGreedApiItem
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("value_classification")]
        public string ValueClassification { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
    }
    #endregion
}