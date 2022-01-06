using System.Windows.Controls;
using ITTV.WPF.MVVM.ViewModels;

namespace ITTV.WPF.MVVM.Views
{
    public partial class GamesView : UserControl
    {
        public GamesView(GamesViewModel gamesViewModel)
        {
            InitializeComponent();
            DataContext = gamesViewModel;
        }
    }
}