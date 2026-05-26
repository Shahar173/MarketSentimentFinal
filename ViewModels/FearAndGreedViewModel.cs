using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MarketSentimentFinal.ViewModels
{
    // מחלקת מודל ייעודית לייצוג שורה בטבלה ההיסטורית
    public class HistoricalIndexItem
    {
        public string DayName { get; set; } = string.Empty;
        public string Value { get; set; } = "50";
        public string Classification { get; set; } = "Neutral";
        public Color ClassificationColor { get; set; } = Colors.Gray;
    }

    public class FearAndGreedViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new();

        private int _currentIndexValue;
        private string _indexClassificationText;
        private string _explanationText;
        private Color _indexColorGlow;
        private ObservableCollection<HistoricalIndexItem> _historicalList = new();
        private bool _isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoBackCommand { get; }
        public ICommand RefreshCommand { get; }

        public FearAndGreedViewModel()
        {
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            RefreshCommand = new Command(async () => await LoadApiDataAsync());

            EvaluateFearGreedMatrix(50);
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

        public ObservableCollection<HistoricalIndexItem> HistoricalList
        {
            get => _historicalList;
            set { _historicalList = value; OnPropertyChanged(); }
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
                var response = await _httpClient.GetFromJsonAsync<AlternativeMeResult>("https://api.alternative.me/fng/?limit=7");

                if (response?.Data != null && response.Data.Count > 0)
                {
                    // עדכון המדד הראשי העדכני של היום
                    var latest = response.Data[0];
                    if (int.TryParse(latest.Value, out int score))
                    {
                        EvaluateFearGreedMatrix(score);
                    }

                    var tempLogList = new List<HistoricalIndexItem>();

                    for (int i = 0; i < response.Data.Count; i++)
                    {
                        var item = response.Data[i];
                        int.TryParse(item.Value, out int dayScore);

                        // המרת חותמת הזמן (Unix Timestamp) ליום בשבוע בזמן ישראל
                        string dayLabel;
                        if (i == 0)
                        {
                            dayLabel = "Today";
                        }
                        else if (i == 1)
                        {
                            dayLabel = "Yesterday";
                        }
                        else if (long.TryParse(item.Timestamp, out long unixTime))
                        {
                            var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
                            dayLabel = dateTime.ToString("dddd"); // מחזיר Sunday, Monday וכו'
                        }
                        else
                        {
                            dayLabel = $"-{i}d";
                        }

                        // הוספה לרשימה הזמנית עם התאמת צבעים מלאה לכל שורה
                        tempLogList.Add(new HistoricalIndexItem
                        {
                            DayName = dayLabel,
                            Value = item.Value,
                            Classification = item.ValueClassification,
                            ClassificationColor = GetMatrixColor(dayScore)
                        });
                    }

                    HistoricalList = new ObservableCollection<HistoricalIndexItem>(tempLogList);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FEAR&GREED API] Error processing data: {ex.Message}");
                EvaluateFearGreedMatrix(50);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private Color GetMatrixColor(int numericScore)
        {
            if (numericScore <= 25) return Color.FromArgb("#FF5252");  // Extreme Fear
            if (numericScore <= 45) return Color.FromArgb("#FFAB40");  // Fear
            if (numericScore <= 55) return Color.FromArgb("#B0BEC5");  // Neutral
            if (numericScore <= 75) return Color.FromArgb("#00E676");  // Greed
            return Color.FromArgb("#00E5FF");                         // Extreme Greed
        }

        private void EvaluateFearGreedMatrix(int numericScore)
        {
            CurrentIndexValue = numericScore;
            IndexColorGlow = GetMatrixColor(numericScore);

            if (numericScore <= 25)
            {
                IndexClassificationText = "Extreme Fear";
                ExplanationText = "Market participant panic is driving deep asset deviations away from their baseline macro curves. API analysis detects severe capital liquidations.";
            }
            else if (numericScore <= 45)
            {
                IndexClassificationText = "Fear";
                ExplanationText = "Capital inflows are running low due to widespread distribution anxieties. Volume metrics show patterns of strategic wallet accumulation.";
            }
            else if (numericScore <= 55)
            {
                IndexClassificationText = "Neutral";
                ExplanationText = "Trading ranges remain bound with light volume distributions. Ecosystem triggers are holding steady while awaiting macro data trends.";
            }
            else if (numericScore <= 75)
            {
                IndexClassificationText = "Greed";
                ExplanationText = "Buy wall structural expansion accelerates as volume scales over short-term resistances. Capital speed shows high momentum, retail FOMO is mounting.";
            }
            else
            {
                IndexClassificationText = "Extreme Greed";
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