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
        string backContent;

        public TimeTableList(List<string> content, string bakcPath)
        {
            timeTable = TimeTable.Instance;

            InitializeComponent();

            this.content = content;
            this.backContent = bakcPath;

            allControl.ItemTemplate = (DataTemplate)FindResource("AllTimeTableTemplate");
            itemsControl.ItemTemplate = (DataTemplate)FindResource("CoursesTemplate");
            Back.ItemTemplate = (DataTemplate)FindResource("BackTemplate");

            PutContent(content, backContent);
        }

        private async void itemsControl_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            string dataItem = button.DataContext as string;

            TimeTableList newContent = await timeTable.Choose(dataItem);
            if (newContent != null) {
                PutContent(newContent.content, newContent.backContent);
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.Instance.UIInvoked();
        }

        private void allControl_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.NavigateTo(new ScrollViewerSample(timeTable.GetImageSourceTimeTable()));
        }

        private async void Back_Click(object sender, RoutedEventArgs e)
        {
            TimeTable.Instance.UnChoose();

            TimeTableList tmp = await TimeTable.Instance.GetContent();
            PutContent(tmp.content, tmp.backContent);
        }

        private void PutContent(List<string> content, string backMessage)
        {
            itemsControl.ItemsSource = content;
            
            if (!string.IsNullOrWhiteSpace(backMessage))
            {
                Back.ItemsSource = new List<string>() { backMessage };
                Back.Visibility = Visibility.Visible;
            }
            else
            {
                Back.Visibility = Visibility.Collapsed;
            }

            allControl.ItemsSource = new List<string>() { "Расписание всего курса" };
            AllCourseColumn.Width = new GridLength(timeTable.GetAll() ? 1 : 0, GridUnitType.Star);
        }
    }
}
