using System.Globalization;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Converters // זה ה-Namespace המדויק
{
    public class BuySellColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // אם הערך הוא מחרוזת (ה-DisplayType שהגדרנו)
            if (value is string type)
            {
                if (type == "BUY") return Color.FromArgb("#22C55E"); // ירוק
                if (type == "SELL") return Color.FromArgb("#EF4444"); // אדום
                return Color.FromArgb("#EAB308"); // צהוב ל-WHALE MOVEMENT
            }

            // fallback לבדיקה בוליאנית (אם במקרה נשלח bool)
            if (value is bool isBuy && isBuy) return Color.FromArgb("#22C55E");
            return Color.FromArgb("#EF4444");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}