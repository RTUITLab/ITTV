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
using static Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models.DataBase;

namespace Microsoft.Samples.Kinect.ControlsBasics.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewsList.xaml
    /// </summary>
    public partial class NewsList : UserControl
    {
        public NewsList()
        {
            InitializeComponent();

            DataCollection<object> dataCollection = DataSource.GetGroup("News");
            itemsControl.ItemTemplate = (DataTemplate)FindResource(dataCollection.TypeGroup + "Template");
            itemsControl.ItemsSource = dataCollection;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.UIInvoked();
            var button = (Button)e.OriginalSource;
            News dataItem = button.DataContext as News;

            if (dataItem != null)
            {
                if (dataItem.NavigationPage != null)
                {
                    MainWindow.history.Add(dataItem.UniqueId);
                    MainWindow.var_navigationRegion.Content = Activator.CreateInstance(dataItem.NavigationPage, dataItem);
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.UIInvoked();
        }
    }
}
