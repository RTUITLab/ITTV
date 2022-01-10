using System;
using System.Windows;
using System.Windows.Controls;

namespace ITTV.WPF.MVVM.Views.Helpers
{
    public static class ImageHelper
    {
        private static void SourceResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Image element)
            {
                element.SetResourceReference(Image.SourceProperty, e.NewValue);
            }
        }

        public static readonly DependencyProperty SourceResourceKeyProperty = DependencyProperty.RegisterAttached(
            "SourceResourceKey",
            typeof(object),
            typeof(ImageHelper),
            new PropertyMetadata(String.Empty, SourceResourceKeyChanged));

        public static void SetSourceResourceKey(Image element, object value)
        {
            element.SetValue(SourceResourceKeyProperty, value);
        }

        public static object GetSourceResourceKey(Image element)
        {
            return element.GetValue(SourceResourceKeyProperty);
        }
    }
}