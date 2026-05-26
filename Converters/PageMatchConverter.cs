using System.Globalization;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Converters
{
    public class PageMatchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // בדיקה האם העמוד הפעיל הנוכחי (value) תואם לעמוד שהוגדר בפרמטר של הכפתור (parameter)
            // מחזיר true אם יש התאמה ו-false אם אין
            return value?.ToString() == parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}