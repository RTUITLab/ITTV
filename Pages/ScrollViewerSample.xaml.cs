//------------------------------------------------------------------------------
// <copyright file="ScrollViewerSample.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for ScrollViewerSample
    /// </summary>
    public partial class ScrollViewerSample : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewerSample"/> class.
        /// </summary>
        public ScrollViewerSample(string ImageSource)
        {
            this.InitializeComponent();

            BitmapImage bi3 = new BitmapImage();

            bi3.BeginInit();
            bi3.UriSource = new Uri(ImageSource);
            bi3.EndInit();

            ImageField.Source = bi3;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.UIInvoked();
        }
    }
}
