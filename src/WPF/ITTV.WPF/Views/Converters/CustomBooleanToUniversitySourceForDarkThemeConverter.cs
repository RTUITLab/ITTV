using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ITTV.WPF.Views.Converters
{
    public class CustomBooleanToUniversitySourceForDarkThemeConverter : IValueConverter
    {
        private const string UniversityLogoResourceName = "ITLogo";
        private const string UniversityBlackLogoResourceName = "BlackITLogo";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool isDarkTheme)
                throw new NotSupportedException();
            
            var resourceName = isDarkTheme ? UniversityBlackLogoResourceName : UniversityLogoResourceName;
            return Application.Current.FindResource(resourceName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}