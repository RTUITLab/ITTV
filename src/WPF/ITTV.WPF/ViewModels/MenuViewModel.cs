using System.Windows.Input;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Commands;
using ITTV.WPF.ViewModels.Games;
using ITTV.WPF.ViewModels.News;
using ITTV.WPF.ViewModels.Schedule;
using ITTV.WPF.ViewModels.Videos;

namespace ITTV.WPF.ViewModels
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