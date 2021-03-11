using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Логика взаимодействия для FrameContainer.xaml
    /// </summary>
    public partial class FrameContainer : Page
    {
        public FrameContainer()
        {
            InitializeComponent();

            scrollViewer.ScrollChanged += (sender, e) => { MainWindow.UIInvoked(); };
        }
    }
}
