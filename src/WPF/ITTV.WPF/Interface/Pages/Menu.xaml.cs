using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly TimeTable _timeTable;
        private readonly CreateData _createData;
        
        public Menu(TimeTable timeTable, CreateData createData, MainWindow mainWindow)
        {
            _timeTable = timeTable;
            _createData = createData;
            
            _mainWindow = mainWindow;

            InitializeComponent();

            var localDataSource = DataSource.GetGroup("Menu");
            itemsControl.ItemTemplate = (DataTemplate)FindResource(localDataSource.TypeGroup + "Template");
            itemsControl.ItemsSource = localDataSource;

            Loaded += (sender, e) =>
            {
                _timeTable.CloseTimeTable();
            };
        }

        private async void UniformGrid_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.UiInvoked();
            var button = (Button)e.OriginalSource;
            DataBase dataBase = button.DataContext as DataBase;

            if (dataBase != null) {
                switch (dataBase.Title)
                {
                    case "Новости":
                        _mainWindow.ContentV2.NavigateTo(new NewsList(_mainWindow));
                        break;
                    case "Видео":
                        _createData.GetAllVideos();
                        _mainWindow.ContentV2.NavigateTo(new VideoList(_mainWindow));
                        break;
                    case "Расписание":
                        _mainWindow.ContentV2.NavigateTo(await _timeTable.GetContent());
                        break;
                    case "Игры":
                        _mainWindow.ContentV2.NavigateTo(new GamesList(_mainWindow));
                        break;
                }
            }
        }
    }
}
