using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ITTV.WPF.MVVM.Views.Converters
{
    public class CustomInverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool customizedBool)
            {
                return customizedBool ? Visibility.Hidden : Visibility.Visible;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Hidden;
            }
            throw new NotSupportedException();
        }
    }
}