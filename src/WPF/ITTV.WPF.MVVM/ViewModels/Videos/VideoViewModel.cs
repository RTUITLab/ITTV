using System;
using System.Windows.Input;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands.Videos;

namespace ITTV.WPF.MVVM.ViewModels.Videos
{
    public class VideoViewModel : ViewModelBase
    {
        public ICommand SelectVideoCommand { get; }
        public ICommand VideoEndedCommand { get; }
        public ICommand VideoRestartedCommand { get; }

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
            VideoEndedCommand = new VideoEndedCommand(this);
            VideoRestartedCommand = new VideoRestartedCommand(this);
            
            Title = title;
            Source = source;
        }

        public void EnableAction()
            => CanDoAction = true;

        public void DisableAction()
            => CanDoAction = false;
        
        public void RestartVideo()
        {
            var source = Source;
            
            Source = null;
            Source = source;
        }
    }
}