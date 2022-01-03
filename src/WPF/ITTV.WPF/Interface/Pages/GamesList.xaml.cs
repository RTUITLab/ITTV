using System.Diagnostics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamesList.xaml
    /// </summary>
    public partial class GamesList : UserControl
    {
        public GamesList()
        {
            InitializeComponent();

            var group = DataSource.GetGroup("Games");
            itemsControl.ItemTemplate = (DataTemplate)FindResource(group.TypeGroup + "Template");
            itemsControl.ItemsSource = group;
        }

        private void UniformGrid_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            DataBase dataBase = button.DataContext as DataBase;
            DataExecuteBase dataExecuteBase = (DataExecuteBase)dataBase;

            if (dataExecuteBase.Parameters != null)
            {
                MainWindow.Instance.ControlsBasicsWindow.Topmost = false;

                Process game = new Process();
                game.StartInfo.FileName = dataExecuteBase.Parameters[0];
                game.EnableRaisingEvents = true;
                game.Exited += (InnerSender, InnerE) => { MainWindow.Instance.Ui(() => { MainWindow.Instance.ControlsBasicsWindow.Topmost = true; }); };
                game.Start();

                ButtonAutomationPeer peer = new ButtonAutomationPeer(MainWindow.Instance.backButton);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainWindow.Instance.UiInvoked();
        }
    }
}
