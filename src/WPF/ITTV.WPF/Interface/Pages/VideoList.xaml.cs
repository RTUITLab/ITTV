using System;
using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.Interface.Pages
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
                if (DataPageBase.Task == DataBase.TaskType.Page && sampleDataItem.NavigationPage != null)
                {
                    MainWindow.Instance.content.NavigateTo((UserControl) Activator.CreateInstance(sampleDataItem.NavigationPage, sampleDataItem.Parameters));
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.Instance.UiInvoked();
        }
    }
}
