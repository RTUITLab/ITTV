using System.Windows.Controls;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Views
{
    public partial class BackgroundVideoView : UserControl
    {
        public BackgroundVideoView(BackgroundVideoViewModel backgroundVideoViewModel)
        {
            InitializeComponent();
            DataContext = backgroundVideoViewModel;
        }

        public BackgroundVideoView()
        {
            InitializeComponent();
        }
    }
}