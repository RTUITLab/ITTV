using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ITTV.WPF.MVVM.Services;
using ITTV.WPF.MVVM.Stores;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class VideosViewModel : ViewModelBase
    {
        private readonly VideosManager _videosManager;
        
        private readonly ObservableCollection<VideoViewModel> _videos = new();
        public ObservableCollection<VideoViewModel> Videos => _videos;


        private readonly NavigationStore _navigationStore;
        
        public VideosViewModel(VideosManager videosManager, 
            NavigationStore navigationStore)
        {
            _videosManager = videosManager;
            _navigationStore = navigationStore;

            SyncVideos();
        }

        private void SyncVideos()
        {
            var videos = _videosManager.GetVideos()
                .Select(x => new VideoViewModel(Path.GetFileName(x.AbsoluteUri), 
                    x,
                    _navigationStore));
            
            foreach (var video in videos)
            {
                _videos.Add(video);
            }
        }
    }
}