using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
using Microsoft.Samples.Kinect.ControlsBasics.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
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
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();

            var localDataSource = DataSource.Instance.GetGroup("Menu");
            itemsControl.ItemTemplate = (DataTemplate)FindResource(localDataSource.TypeGroup + "Template");
            itemsControl.ItemsSource = localDataSource;
        }

        private async void UniformGrid_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.UIInvoked();
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
                        MainWindow.Instance.content.NavigateTo(new TimeTableList());
                        break;
                    case "Игры":
                        MainWindow.Instance.content.NavigateTo(new GamesList());
                        break;
                }
            }
        }
    }
}
