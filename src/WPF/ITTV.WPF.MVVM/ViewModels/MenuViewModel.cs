namespace ITTV.WPF.MVVM.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly TimeTableViewModel _timeTableViewModel;
        private readonly NewsViewModel _newsViewModel;
        private readonly VideosViewModel _videosViewModel;
        private readonly GamesViewModel _gamesViewModel;

        public MenuViewModel(TimeTableViewModel timeTableViewModel, 
            NewsViewModel newsViewModel, 
            VideosViewModel videosViewModel,
            GamesViewModel gamesViewModel)
        {
            _timeTableViewModel = timeTableViewModel;
            _newsViewModel = newsViewModel;
            _videosViewModel = videosViewModel;
            _gamesViewModel = gamesViewModel;
        }
    }
}