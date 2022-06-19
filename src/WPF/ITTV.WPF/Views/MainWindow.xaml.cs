using System.Windows;
using System.Windows.Input;
using ITTV.WPF.Core.Models;
using ITTV.WPF.ViewModels;

namespace ITTV.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = mainViewModel;
            
            mainViewModel.Settings.SettingsUpdated += UpdateConfiguration;
            
            UpdateConfiguration(mainViewModel.Settings);
        }


        private void UpdateConfiguration(Settings settings)
        {
            if (settings.IsAdminMode)
            {
                Cursor = Cursors.Arrow;
                Topmost = false;

                return;
            }

            Cursor = Cursors.None;
            Topmost = true;
        }
    }
}