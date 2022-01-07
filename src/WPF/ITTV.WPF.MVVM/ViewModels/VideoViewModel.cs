using System;
using System.Windows.Input;
using ITTV.WPF.MVVM.Commands.Videos;
using ITTV.WPF.MVVM.Stores;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class VideoViewModel : ViewModelBase
    {
        public ICommand SelectVideoCommand { get; }
        public ICommand VideoStageChangedCommand { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (Equals(_title, value))
                    return;

                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private Uri _source;
        public Uri Source
        {
            get => _source;
            private set
            {
                if (Equals(_source, value))
                    return;

                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }

        private bool _canDoAction;
        public bool CanDoAction
        {
            get => _canDoAction;
            private set
            {
                if (Equals(_canDoAction, value))
                    return;

                _canDoAction = value;
                OnPropertyChanged(nameof(CanDoAction));
            }
        }
        
        public VideoViewModel(string title, Uri source,
            NavigationStore navigationStore)
        {
            SelectVideoCommand = new SelectVideoCommand(this, navigationStore);
            VideoStageChangedCommand = new VideoStageChangedCommand(this);
            
            Title = title;
            Source = source;
        }

        public void StartVideoAction()
        {
            CanDoAction = !_canDoAction;
            StartVideo();

        }

        private void StartVideo()
        {
            var source = Source;
            
            Source = null;
            Source = source;
        }
    }
}