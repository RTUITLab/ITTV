using System.Windows.Input;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.MVVM.Commands;
using ITTV.WPF.MVVM.ViewModels.Games;
using ITTV.WPF.MVVM.ViewModels.News;
using ITTV.WPF.MVVM.ViewModels.Schedule;
using ITTV.WPF.MVVM.ViewModels.Videos;

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