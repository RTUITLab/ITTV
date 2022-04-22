using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ITTV.WPF.Views.Converters
{
    public class CustomBooleanToColorForDarkThemeTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDarkTheme)
            {
                return isDarkTheme ? Brushes.White : Brushes.Black;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}