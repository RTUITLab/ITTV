using System.Diagnostics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Views;

namespace ITTV.WPF.Interface.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamesList.xaml
    /// </summary>
    public partial class GamesList : UserControl
    {
        private readonly MainWindow _mainWindow;
        public GamesList(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            
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
                _mainWindow.ControlsBasicsWindow.Topmost = false;

                Process game = new Process();
                game.StartInfo.FileName = dataExecuteBase.Parameters[0];
                game.EnableRaisingEvents = true;
                game.Exited += (InnerSender, InnerE) => { _mainWindow.Ui(() => { _mainWindow.ControlsBasicsWindow.Topmost = true; }); };
                game.Start();

                ButtonAutomationPeer peer = new ButtonAutomationPeer(_mainWindow.backButton);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _mainWindow.UiInvoked();
        }
    }
}
