using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;

namespace Microsoft.Samples.Kinect.ControlsBasics.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewsList.xaml
    /// </summary>
    public partial class VideoList : UserControl
    {
        public VideoList()
        {
            InitializeComponent();

            this.itemsControl.ItemTemplate = (DataTemplate)this.FindResource(DataSource.GetGroup("Video").TypeGroup + "Template");
            this.itemsControl.ItemsSource = DataSource.GetGroup("Video");
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {

            var button = (Button)e.OriginalSource;
            Video sampleDataItem = button.DataContext as Video;

            if (sampleDataItem != null)
            {
                if (sampleDataItem.Task == DataBase.TaskType.Page && sampleDataItem.NavigationPage != null)
                {
                    MainWindow.history.Add(sampleDataItem.UniqueId);
                    MainWindow.var_navigationRegion.Content = Activator.CreateInstance(sampleDataItem.NavigationPage, sampleDataItem.Parametrs);
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.UIInvoked();
        }
    }
}
