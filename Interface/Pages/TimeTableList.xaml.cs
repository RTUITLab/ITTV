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
        public TimeTableList(List<string> content)
        {
            InitializeComponent();

            DataTemplate template = (DataTemplate)FindResource("CoursesTemplate");
            itemsControl.ItemTemplate = template;
            itemsControl.ItemsSource = content;
        }

        public TimeTableList(List<Lesson> content)
        {
            InitializeComponent();

            DataTemplate template = (DataTemplate)FindResource("CoursesTemplate");
            itemsControl.ItemTemplate = template;
            itemsControl.ItemsSource = content;
        }

        private async void itemsControl_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.OriginalSource;
            TextBlock textBlock = (TextBlock)button.FindName("Title");
            string text = textBlock.Text;
            TimeTable.Instance.Choose(text);
            MainWindow.Instance.content.NavigateTo(await TimeTable.Instance.GetContent());
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.Instance.UIInvoked();
        }
    }
}
