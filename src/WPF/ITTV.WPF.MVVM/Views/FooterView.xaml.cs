using System.Windows.Controls;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Views
{
    public partial class FooterView : UserControl
    {
        public FooterView(FooterViewModel footerViewModel)
        {
            InitializeComponent();
            DataContext = footerViewModel;
        }

        public FooterView()
        {
            InitializeComponent();
        }
    }
}