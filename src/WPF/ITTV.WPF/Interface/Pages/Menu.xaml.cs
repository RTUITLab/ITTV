using System.Windows;
using System.Windows.Controls;
using ITTV.WPF.DataModel;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();

            var localDataSource = DataSource.GetGroup("Menu");
            itemsControl.ItemTemplate = (DataTemplate)FindResource(localDataSource.TypeGroup + "Template");
            itemsControl.ItemsSource = localDataSource;

            Loaded += (sender, e) => { TimeTable.Instance.CloseTimeTable(); };
        }

        private async void UniformGrid_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.UiInvoked();
            var button = (Button)e.OriginalSource;
            DataBase dataBase = button.DataContext as DataBase;

            if (dataBase != null) {
                switch (dataBase.Title)
                {
                    case "Новости":
                        MainWindow.Instance.content.NavigateTo(new NewsList());
                        break;
                    case "Видео":
                        CreateData.Instance.GetAllVideos();
                        MainWindow.Instance.content.NavigateTo(new VideoList());
                        break;
                    case "Расписание":
                        MainWindow.Instance.content.NavigateTo(await TimeTable.Instance.GetContent());
                        break;
                    case "Игры":
                        MainWindow.Instance.content.NavigateTo(new GamesList());
                        break;
                }
            }
        }
    }
}
