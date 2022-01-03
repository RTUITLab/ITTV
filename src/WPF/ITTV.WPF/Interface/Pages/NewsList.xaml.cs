using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.Interface.Pages
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
            MainWindow.Instance.UiInvoked();
            var button = (Button)e.OriginalSource;
            News dataItem = button.DataContext as News;

            if (dataItem != null)
            {
                 MainWindow.Instance.content.NavigateTo(new NewsPage(dataItem));
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.Instance.UiInvoked();
        }
    }
}
