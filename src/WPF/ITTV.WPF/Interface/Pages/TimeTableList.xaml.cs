using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для TimeTableList.xaml
    /// </summary>
    public partial class TimeTableList : UserControl
    {
        List<string> content;
        private string backContent;
        private readonly TimeTable _timeTable;
        private readonly MainWindow _mainWindow;

        public TimeTableList(List<string> content, string bakcPath, TimeTable timeTable, MainWindow mainWindow)
        {
            InitializeComponent();

            this.content = content;
            _timeTable = timeTable;
            _mainWindow = mainWindow;
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

            var newContent = await _timeTable.Choose(dataItem);
            if (newContent != null) {
                PutContent(newContent.content, newContent.backContent);
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _mainWindow.UiInvoked();
        }

        private void allControl_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ContentV2.NavigateTo(new ScrollViewerSample(_timeTable.GetImageSourceTimeTable(), _mainWindow));
        }

        private async void Back_Click(object sender, RoutedEventArgs e)
        {
            _timeTable.UnChoose();

            var tmp = await _timeTable.GetContent();
            PutContent(tmp.content, tmp.backContent);
        }

        private void PutContent(IEnumerable<string> content, string backMessage)
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
            AllCourseColumn.Width = new GridLength(_timeTable.GetAll() ? 1 : 0, GridUnitType.Star);
        }
    }
}
