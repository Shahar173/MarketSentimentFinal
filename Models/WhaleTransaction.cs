using System;
using Microsoft.Maui.Graphics;

namespace MarketSentimentFinal.Models
{
    public class WhaleTransaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Coin { get; set; } = "BTC";
        public string AmountText { get; set; } = "0";
        public string FullAmountText { get; set; } = "0"; // תוקן: כמות מלאה למסך הפירוט
        public double AmountUSD { get; set; }
        public string FromAddress { get; set; } = "Unknown Wallet";
        public string ToAddress { get; set; } = "Unknown Wallet";
        public string FromAddressHash { get; set; } = string.Empty; // תוקן: האש מלא של השולח
        public string ToAddressHash { get; set; } = string.Empty;   // תוקן: האש מלא של המקבל
        public string TransactionHash { get; set; } = string.Empty; // תוקן: מזהה העסקה בבלוקצ'יין
        public string TransactionType { get; set; } = "Wallet to Exchange";
        public string TimeAgo { get; set; } = "5m ago";

        public bool IsBuy => TransactionType.ToLower().Contains("exchange to wallet") || TransactionType.ToLower().Contains("minted");
    }
}