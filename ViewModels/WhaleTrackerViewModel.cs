using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MarketSentimentFinal.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MarketSentimentFinal.ViewModels
{
    public class WhaleTrackerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<WhaleTransaction> _transactions = new();
        private bool _isLoading;
        
        // פרופרטיז עבור הדאשבורד העליון
        private int _sentimentScore;
        private string _sentimentStatusText = "NEUTRAL";
        private double _indicatorPosition;
        private string _buyVolumeText = "$0";
        private string _sellVolumeText = "$0";
        private int _buyTxnsCount;
        private int _sellTxnsCount;
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
        public string BuyVolumeText { get => _buyVolumeText; set { _buyVolumeText = value; OnPropertyChanged(); } }
        public string SellVolumeText { get => _sellVolumeText; set { _sellVolumeText = value; OnPropertyChanged(); } }
        public int BuyTxnsCount { get => _buyTxnsCount; set { _buyTxnsCount = value; OnPropertyChanged(); } }
        public int SellTxnsCount { get => _sellTxnsCount; set { _sellTxnsCount = value; OnPropertyChanged(); } }
        public string BuyPercentText { get => _buyPercentText; set { _buyPercentText = value; OnPropertyChanged(); } }
        public string SellPercentText { get => _sellPercentText; set { _sellPercentText = value; OnPropertyChanged(); } }
        public double BuyProgressBarRatio { get => _buyProgressBarRatio; set { _buyProgressBarRatio = value; OnPropertyChanged(); } }
        #endregion

        public WhaleTrackerViewModel()
        {
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
                await Task.Delay(800); // סימולציית רשת קלה

                // נתוני מוק מפורטים תואמים לריפרנס החדש
                var mockData = new List<WhaleTransaction>
                {
                    new WhaleTransaction { Coin = "DOGE", AmountText = "7.77M", AmountUSD = 799340, FromAddress = "Unknown Wallet", ToAddress = "Unknown Wallet", TransactionType = "Wallet to Wallet", TimeAgo = "5m ago" },
                    new WhaleTransaction { Coin = "BTC", AmountText = "1,420", AmountUSD = 92300000, FromAddress = "Unknown Wallet", ToAddress = "Binance", TransactionType = "Wallet to Exchange", TimeAgo = "12m ago" },
                    new WhaleTransaction { Coin = "ETH", AmountText = "18,500", AmountUSD = 55200000, FromAddress = "Kraken", ToAddress = "Unknown Wallet", TransactionType = "Exchange to Wallet", TimeAgo = "25m ago" },
                    new WhaleTransaction { Coin = "SOL", AmountText = "125,000", AmountUSD = 22100000, FromAddress = "Unknown Wallet", ToAddress = "Coinbase", TransactionType = "Wallet to Exchange", TimeAgo = "44m ago" },
                    new WhaleTransaction { Coin = "BTC", AmountText = "850", AmountUSD = 55250000, FromAddress = "Binance", ToAddress = "Unknown Wallet", TransactionType = "Exchange to Wallet", TimeAgo = "1h ago" },
                    new WhaleTransaction { Coin = "DOGE", AmountText = "9.04M", AmountUSD = 931000, FromAddress = "Unknown Wallet", ToAddress = "Unknown Wallet", TransactionType = "Wallet to Wallet", TimeAgo = "1h ago" }
                };

                Transactions = new ObservableCollection<WhaleTransaction>(mockData);
                CalculateWhaleSentiment(mockData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WHALE TRACKER] Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CalculateWhaleSentiment(List<WhaleTransaction> list)
        {
            double totalBuyUSD = 0;
            double totalSellUSD = 0;
            int buyCount = 0;
            int sellCount = 0;

            foreach (var tx in list)
            {
                if (tx.IsBuy)
                {
                    totalBuyUSD += tx.AmountUSD;
                    buyCount++;
                }
                else if (tx.TransactionType.ToLower().Contains("to exchange"))
                {
                    totalSellUSD += tx.AmountUSD;
                    sellCount++;
                }
                else
                {
                    // העברות בין ארנקים (Wallet to Wallet) - נחשיב 50/50 נייטרלי או נחלק שווה בשווה
                    totalBuyUSD += tx.AmountUSD * 0.5;
                    totalSellUSD += tx.AmountUSD * 0.5;
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
        }

        private string FormatVolumeText(double val)
        {
            if (val >= 1000000000) return $"${(val / 1000000000):F1}B";
            if (val >= 1000000) return $"${(val / 1000000):F1}M";
            return $"${val:N0}";
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}