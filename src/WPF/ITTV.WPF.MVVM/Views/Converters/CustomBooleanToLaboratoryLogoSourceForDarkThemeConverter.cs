using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ITTV.WPF.MVVM.Views.Converters
{
    public class CustomBooleanToLaboratoryLogoSourceForDarkThemeConverter : IValueConverter
    {
        private const string LaboratoryLogoResource = "RTUITLabLogo";
        private const string LaboratoryBlackLogoResource = "BlackRTUITLabLogo";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool isDarkTheme)
                throw new NotSupportedException();
            
            var resourceName = isDarkTheme ? LaboratoryBlackLogoResource : LaboratoryLogoResource;
            return Application.Current.FindResource(resourceName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}