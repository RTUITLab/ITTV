using ITTV.WPF.MVVM.ViewModels;
using Microsoft.Kinect.Wpf.Controls;

namespace ITTV.WPF.MVVM.Views
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
        }
    }
}