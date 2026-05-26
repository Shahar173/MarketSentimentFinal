using System;
using System.Text.Json.Serialization;

namespace MarketSentimentFinal.Models
{
    public class WhaleTransaction
    {
        // קבוצה 1: נתונים בסיסיים של ההעברה (סכום בדולרים, סוג המטבע, כתובות הארנקים וזמן)
        public double AmountUSD { get; set; }
        public string Coin { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string TimeAgo { get; set; }
        public string AmountText { get; set; }

        // קבוצה 2: סוג הפעולה הגולמי שמגיע מה-API (למשל העברה לבורסה או מחוצה לה)
        public string TransactionType { get; set; }

        // קבוצה 3: מאפיינים מחושבים (Properties) שמתרגמים את סוג הפעולה למושגי מסחר (BUY/SELL) עבור ה-UI והקונברטר
        public string DisplayType => TransactionType switch
        {
            "exchange_outflow" => "BUY",       // יציאה מבורסה לארנק פרטי = קנייה/אגירה
            "exchange_inflow" => "SELL",       // כניסה מארנק פרטי לבורסה = מכירה
            _ => "WHALE MOVEMENT"              // העברה כללית בין ארנקים
        };

        public bool IsBuy => TransactionType == "exchange_outflow";
    }
}