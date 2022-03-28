using System.Windows.Input;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.BackgroundVideos;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class EggVideoViewModel : ViewModelBase
    { 
        private readonly Settings _settings;
        
        public double Volume => _settings.VideoVolume;
        public static string VideoPath => PathHelper.FileEggVideoPath;
        public ICommand NavigateBackgroundVideo { get; }

        public EggVideoViewModel(NavigationStore navigationStore,
            Settings settings, UserInterfaceManager userInterfaceManager)
        {
            NavigateBackgroundVideo = new NavigateBackgroundVideoAndClearHistoryCommand(navigationStore, userInterfaceManager);
            
            _settings = settings;
            _settings.SettingsUpdated += _ => OnSettingsUpdated();
        }

        private void OnSettingsUpdated()
        {
            OnPropertyChanged(nameof(Volume));
        }
    }
}