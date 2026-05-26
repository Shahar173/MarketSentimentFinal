using System.Globalization;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Converters
{
    public class BuySellColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // בדיקת התאמה עבור ערך מסוג מחרוזת והחזרת צבע לפי סוג הפעולה
            if (value is string type)
            {
                if (type == "BUY") return Color.FromArgb("#22C55E"); // ירוק לפעולת קנייה
                if (type == "SELL") return Color.FromArgb("#EF4444"); // אדום לפעולת מכירה
                return Color.FromArgb("#EAB308"); // צהוב לפעולות כלליות של תנועת לווייתנים
            }

            // בדיקת גיבוי למקרה שהערך התקבל כמשתנה בוליאני
            if (value is bool isBuy && isBuy) return Color.FromArgb("#22C55E");
            return Color.FromArgb("#EF4444");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}