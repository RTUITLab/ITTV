using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using Microsoft.Samples.Kinect.ControlsBasics.Network;
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

namespace Microsoft.Samples.Kinect.ControlsBasics.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для TimeTableList.xaml
    /// </summary>
    public partial class TimeTableList : UserControl
    {
        TimeTable timeTable = TimeTable.Instance;

        public TimeTableList()
        {
            InitializeComponent();

            DataTemplate template = (DataTemplate)FindResource("CoursesTemplate");
            itemsControl.ItemTemplate = template;
            itemsControl.ItemsSource = timeTable.GetContent();

            if (timeTable.GetAll()) {
                allControl.ItemTemplate = template;
                allControl.ItemsSource = "Расписание всего курса";
                allControl.Visibility = Visibility.Visible;
            } else
            {
                allControl.Visibility = Visibility.Collapsed;
            }
        }

        private void itemsControl_Click(object sender, RoutedEventArgs e)
        {
            //((ItemsControl) sender).ItemsSource
            timeTable.Choose(0);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.Instance.UIInvoked();
        }

        private void allControl_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.NavigateTo(new ScrollViewerSample(timeTable.GetImageSourceTimeTable()));
        }
    }
}
