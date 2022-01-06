using System.Windows.Controls;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Views
{
    public partial class NewsView : UserControl
    {
        public NewsView(NewsViewModel newsViewModel)
        {
            InitializeComponent();
            DataContext = newsViewModel;
        }
    }
}