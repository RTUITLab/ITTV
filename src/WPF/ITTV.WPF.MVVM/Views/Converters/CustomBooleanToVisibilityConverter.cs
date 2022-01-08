﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ITTV.WPF.MVVM.Views.Converters
{
    public class CustomBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool customizedBool)
            {
                return customizedBool ? Visibility.Visible : Visibility.Hidden;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            throw new NotSupportedException();
        }
    }
}