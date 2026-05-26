using System;
using System.Text.Json.Serialization; // חובה להוסיף את זה

namespace MarketSentimentFinal.Models
{
    public class WhaleTransaction
    {
        public double AmountUSD { get; set; }
        public string Coin { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string TimeAgo { get; set; } // הוספנו set כדי שיהיה ניתן לכתיבה
        public string AmountText { get; set; }

        // אלו השדות שחסרו והשגיאות שראית:
        public string TransactionType { get; set; }

        public string DisplayType => TransactionType switch
        {
            "exchange_outflow" => "BUY",
            "exchange_inflow" => "SELL",
            _ => "WHALE MOVEMENT" 
        };

        public bool IsBuy => TransactionType == "exchange_outflow";
    }
}