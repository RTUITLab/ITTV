using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Stores;

namespace ITTV.WPF.MVVM.ViewModels.Videos
{
    public class VideosViewModel : ViewModelBase
    {
        private readonly VideosManager _videosManager;
        
        private ObservableCollection<VideoViewModel> _videos = new();
        public ObservableCollection<VideoViewModel> Videos
        {
            get => _videos;
            set
            {
                if (Equals(_videos, value))
                    return;

                if (_videos != null && value != null)
                {
                    if (_videos.SequenceEqual(value))
                        return;
                }

                _videos = value;
                OnPropertyChanged(nameof(Videos));
            }
        }


        private readonly NavigationStore _navigationStore;
        
        public VideosViewModel(VideosManager videosManager, 
            NavigationStore navigationStore)
        {
            _videosManager = videosManager;
            _navigationStore = navigationStore;
        }

        public override Task Recalculate()
        {
            SetUnloaded();
            
            var videos = _videosManager.GetVideos()
                .Select(x => new VideoViewModel(Path.GetFileNameWithoutExtension(x.OriginalString), 
                    x,
                    _navigationStore));

            Videos = new ObservableCollection<VideoViewModel>(videos);

            SetLoaded();
            
            return Task.CompletedTask;
        }
    }
}