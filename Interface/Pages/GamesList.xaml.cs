using Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models;
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
    /// Логика взаимодействия для GamesList.xaml
    /// </summary>
    public partial class GamesList : UserControl
    {
        public GamesList()
        {
            InitializeComponent();

            var group = DataSource.Instance.GetGroup("Games");
            itemsControl.ItemTemplate = (DataTemplate)FindResource(group.TypeGroup + "Template");
            itemsControl.ItemsSource = group;
        }

        private void UniformGrid_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            DataBase dataBase = button.DataContext as DataBase;
            DataExecuteBase dataExecuteBase = (DataExecuteBase)dataBase;

            if (dataExecuteBase.Parametrs != null)
            {
                MainWindow.Instance.ControlsBasicsWindow.Topmost = false;

                Process game = new Process();
                game.StartInfo.FileName = dataExecuteBase.Parametrs[0];
                game.EnableRaisingEvents = true;
                game.Exited += (InnerSender, InnerE) => { MainWindow.Instance.UI(() => { MainWindow.Instance.ControlsBasicsWindow.Topmost = true; }); };
                game.Start();

                ButtonAutomationPeer peer = new ButtonAutomationPeer(MainWindow.Instance.backButton);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.Instance.UIInvoked();
        }
    }
}
