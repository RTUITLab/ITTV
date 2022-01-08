using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Services;

namespace ITTV.WPF.Views
{
    public class MainWindowViewModel
    {
        private readonly Settings _settings;
        private readonly MireaTimeManager _mireaTimeManager;
        public MainWindowViewModel(MireaTimeManager timeManager, Settings settings, MireaTimeManager mireaTimeManager)
        {
            _settings = settings;
            _mireaTimeManager = mireaTimeManager;
        }
    }
}