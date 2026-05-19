using System.Globalization;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Converters // This MUST match the clr-namespace in your XAML
{
    public class PageMatchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}