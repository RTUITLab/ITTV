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
        TimeTable timeTable;
        List<string> content;

        public TimeTableList(List<string> content)
        {
            InitializeComponent();

            this.content = content;
            Loaded += TimeTableList_Loaded;
        }

        private void TimeTableList_Loaded(object sender, RoutedEventArgs e)
        {
            timeTable = TimeTable.Instance; 

            itemsControl.ItemTemplate = (DataTemplate)FindResource("CoursesTemplate");
            itemsControl.ItemsSource = content;

            if (timeTable.GetAll())
            {
                allControl.ItemTemplate = (DataTemplate)FindResource("AllCoursesTemplate");
                allControl.ItemsSource = new List<string>(){ "Расписание всего курса" };
                allControl.Visibility = Visibility.Visible;
            }
            else
            {
                allControl.Visibility = Visibility.Collapsed;
            }
        }

        private void itemsControl_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            string dataItem = button.DataContext as string;
            timeTable.Choose(dataItem);
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
