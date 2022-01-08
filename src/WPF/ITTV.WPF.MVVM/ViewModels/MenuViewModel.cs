using System.Windows.Input;
using ITTV.WPF.MVVM.Commands;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public ICommand ShowTimeTableCommand { get; }
        public ICommand ShowNewsCommand { get; }
        public ICommand ShowVideosCommand { get; }
        public ICommand ShowGamesCommand { get; }

        public MenuViewModel(NavigateCommand<TimeTableViewModel> timeTableNavigateCommand,
            NavigateCommand<NewsViewModel> newsNavigateCommand,
            NavigateCommand<VideosViewModel> videosNavigateCommand,
            NavigateCommand<GamesViewModel> gamesNavigateCommand)
        {
            ShowTimeTableCommand = timeTableNavigateCommand;
            ShowNewsCommand = newsNavigateCommand;
            ShowVideosCommand = videosNavigateCommand;
            ShowGamesCommand = gamesNavigateCommand;
        }
    }
}