using MarketSentimentFinal.Models;
using MarketSentimentFinal.Models; // כדי שהוא יכיר את WhaleResponse
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq;          // חובה כדי ש-Sum ו-Where יעבדו
using System.Net.Http.Json; // חובה כדי ש-GetFromJsonAsync יעבוד
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarketSentimentFinal.ViewModels
{
    public class WhaleTrackerViewModel : INotifyPropertyChanged
    {
        public static string SharedBuyPercent { get; set; } = "50% Buy";
        public static string SharedSellPercent { get; set; } = "50% Sell";
        public static int SharedScore { get; set; } = 50;
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<WhaleTransaction> _transactions = new();
        private bool _isLoading;
        private bool _hasLoaded = false;

        // --- שדות פרטיים (Private Fields) ---
        private readonly HttpClient _httpClient = new HttpClient();
        private int _sentimentScore;
        private string _sentimentStatusText = "NEUTRAL";
        private double _indicatorPosition;
        

        // שדות למחווני נפח עסקאות (בלי כפילויות)
        private string _buyVolumeText = "$0.0M";
        private string _sellVolumeText = "$0.0M";
        private int _buyTxnsCount;
        private int _sellTxnsCount;

        // שדות עזר נוספים
        private string _buyPercentText = "50% Buy";
        private string _sellPercentText = "50% Sell";
        private double _buyProgressBarRatio = 0.5;

        public ICommand GoBackCommand { get; }
        public ICommand RefreshCommand { get; }

        #region Properties
        public ObservableCollection<WhaleTransaction> Transactions { get => _transactions; set { _transactions = value; OnPropertyChanged(); } }
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
        public int SentimentScore { get => _sentimentScore; set { _sentimentScore = value; OnPropertyChanged(); } }
        public string SentimentStatusText { get => _sentimentStatusText; set { _sentimentStatusText = value; OnPropertyChanged(); } }
        public double IndicatorPosition { get => _indicatorPosition; set { _indicatorPosition = value; OnPropertyChanged(); } }
        public string BuyPercentText { get => _buyPercentText; set { _buyPercentText = value; OnPropertyChanged(); } }
        public string SellPercentText { get => _sellPercentText; set { _sellPercentText = value; OnPropertyChanged(); } }
        public double BuyProgressBarRatio { get => _buyProgressBarRatio; set { _buyProgressBarRatio = value; OnPropertyChanged(); } }
        public string BuyVolumeText
        {
            get => _buyVolumeText;
            set { _buyVolumeText = value; OnPropertyChanged(); }
        }

        public string SellVolumeText
        {
            get => _sellVolumeText;
            set { _sellVolumeText = value; OnPropertyChanged(); }
        }

        public int BuyTxnsCount
        {
            get => _buyTxnsCount;
            set { _buyTxnsCount = value; OnPropertyChanged(); }
        }

        public int SellTxnsCount
        {
            get => _sellTxnsCount;
            set { _sellTxnsCount = value; OnPropertyChanged(); }
        }
        #endregion

        public WhaleTrackerViewModel()
        {
            System.Diagnostics.Debug.WriteLine("DEBUG: Constructor called!");

            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            RefreshCommand = new Command(async () => await LoadWhaleDataAsync());

            _ = LoadWhaleDataAsync();
        }

        private async Task LoadWhaleDataAsync()
        {
            if (IsLoading) return;
            IsLoading = true;

            try
            {
                string apiUrl = "https://cryptonews-api.com/api/v1/whale-transactions?tickers=BTC,ETH&date=last24hours&token=2ke2ezrpzznixlsh44l96dl5ivrxcfl31lhubxwd";

                // שימוש ב-NoCache כדי למנוע נתונים תקועים מהזיכרון
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { NoCache = true };

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(jsonString);

                    if (document.RootElement.TryGetProperty("data", out var dataArray))
                    {
                        double buyVol = 0;
                        double sellVol = 0;
                        int buyCount = 0;
                        int sellCount = 0;
                        var newList = new List<WhaleTransaction>();

                        foreach (var item in dataArray.EnumerateArray())
                        {
                            // המרת הסכום בבטחה - תומך גם בטקסט וגם במספר
                            double amount = 0;
                            if (item.TryGetProperty("amount_usd", out var a))
                            {
                                amount = a.ValueKind == JsonValueKind.Number ? a.GetDouble() : double.Parse(a.GetString() ?? "0");
                            }

                            string direction = item.TryGetProperty("direction", out var d) ? d.GetString()?.ToLower() : "unknown";

                            // סיווג מדויק
                            if (direction == "exchange_outflow") { buyVol += amount; buyCount++; }
                            else if (direction == "exchange_inflow") { sellVol += amount; sellCount++; }

                            newList.Add(new WhaleTransaction
                            {
                                AmountUSD = amount,
                                Coin = item.TryGetProperty("ticker", out var tk) ? tk.GetString() : "BTC",
                                FromAddress = item.TryGetProperty("from_address", out var fr) ? fr.GetString() : "Unknown",
                                ToAddress = item.TryGetProperty("to_address", out var to) ? to.GetString() : "Unknown",
                                TimeAgo = item.TryGetProperty("date", out var dt) ? ConvertToIsraelTime(dt.GetString()) : "Recent",
                                AmountText = amount >= 1000000 ? $"${(amount / 1000000):F1}M" : $"${(amount / 1000):F0}K",
                                TransactionType = direction
                            });
                        }

                        // עדכון ה-UI בצורה בטוחה
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            this.BuyVolumeText = $"${(buyVol / 1000000):F1}M";
                            this.SellVolumeText = $"${(sellVol / 1000000):F1}M";
                            this.BuyTxnsCount = buyCount;
                            this.SellTxnsCount = sellCount;

                            // עדכון ה-ObservableCollection בצורה נקייה
                            this.Transactions.Clear();
                            foreach (var tx in newList) this.Transactions.Add(tx);

                            CalculateWhaleSentiment(this.Transactions.ToList());
                        });
                        System.Diagnostics.Debug.WriteLine($"[WHALE API] Updated: {buyCount + sellCount} txns found.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WHALE API] Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private string ConvertToIsraelTime(string rawDate)
        {
            // מנסה להפוך את המחרוזת לזמן
            if (DateTime.TryParse(rawDate, out DateTime dateValue))
            {
                // מוסיף 3 שעות עבור שעון ישראל (כי השרת ב-UTC)
                var israelTime = dateValue.AddHours(3);
                return israelTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return rawDate;
        }

        private void CalculateWhaleSentiment(List<WhaleTransaction> list)
        {
            double totalBuyUSD = 0;
            double totalSellUSD = 0;
            int buyCount = 0;
            int sellCount = 0;

            foreach (var tx in list)
            {
                // אנחנו משתמשים ב-TransactionType שהוא בעצם ה-direction
                if (tx.TransactionType == "exchange_outflow")
                {
                    totalBuyUSD += tx.AmountUSD;
                    buyCount++;
                }
                else if (tx.TransactionType == "exchange_inflow")
                {
                    totalSellUSD += tx.AmountUSD;
                    sellCount++;
                }
            }

            BuyTxnsCount = buyCount;
            SellTxnsCount = sellCount;

            double totalVolume = totalBuyUSD + totalSellUSD;
            if (totalVolume > 0)
            {
                double buyRatio = totalBuyUSD / totalVolume;
                SentimentScore = (int)(buyRatio * 100);
                BuyProgressBarRatio = buyRatio;

                BuyPercentText = $"{ (buyRatio * 100):F1}% Buy";
                SellPercentText = $"{ ((1 - buyRatio) * 100):F1}% Sell";
            }
            else
            {
                SentimentScore = 50;
                BuyProgressBarRatio = 0.5;
            }

            // עדכון המלל והצבע לפי הציון
            if (SentimentScore >= 65) { SentimentStatusText = "STRONG BUY PRESSURE"; }
            else if (SentimentScore >= 55) { SentimentStatusText = "BUY PRESSURE"; }
            else if (SentimentScore >= 45) { SentimentStatusText = "NEUTRAL"; }
            else if (SentimentScore >= 35) { SentimentStatusText = "SELL PRESSURE"; }
            else { SentimentStatusText = "HEAVY SELL PRESSURE"; }

            // פורמט תצוגת נפחים מקוצר (B ו-M)
            BuyVolumeText = FormatVolumeText(totalBuyUSD);
            SellVolumeText = FormatVolumeText(totalSellUSD);

            // חישוב מיקום המחוון (נניח אורך הסקאלה הוא 310 פיקסלים)
            IndicatorPosition = (SentimentScore / 100.0) * 288;

            SharedBuyPercent = BuyPercentText;
            SharedSellPercent = SellPercentText;
            SharedScore = SentimentScore;
        }

        public async Task InitializeAsync()
        {
            if (_hasLoaded) return; // מונע טעינה כפולה
            await LoadWhaleDataAsync();
            _hasLoaded = true;
        }

        private string FormatVolumeText(double val)
        {
            if (val <= 0) return "$0.0M"; // טיפול ב-0
            if (val >= 1000000000) return $"${(val / 1000000000):F1}B";
            if (val >= 1000000) return $"${(val / 1000000):F1}M";
            return $"${(val / 1000):F0}K";
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}