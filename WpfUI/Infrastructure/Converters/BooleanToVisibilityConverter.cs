using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolVal = (bool)value;
            if (boolVal)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibilityVal = (Visibility)value;
            if (visibilityVal == Visibility.Visible)
            {
                return true;
            }

            return true;
        }
    }
}