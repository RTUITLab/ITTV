using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewsList.xaml
    /// </summary>
    public partial class NewsList : UserControl
    {
        private readonly MainWindow _mainWindow;
        public NewsList(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            
            InitializeComponent();

            DataCollection<object> dataCollection = DataSource.GetGroup("News");
            itemsControl.ItemTemplate = (DataTemplate)FindResource(dataCollection.TypeGroup + "Template");
            itemsControl.ItemsSource = dataCollection;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.UiInvoked();
            var button = (Button)e.OriginalSource;
            News dataItem = button.DataContext as News;

            if (dataItem != null)
            {
                _mainWindow.ContentV2.NavigateTo(new NewsPage(dataItem, _mainWindow));
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _mainWindow.UiInvoked();
        }
    }
}
