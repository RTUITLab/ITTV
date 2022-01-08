using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.MVVM.Models;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class GamesViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Game> _games = new();
        public IReadOnlyCollection<Game> Games => _games;

        public GamesViewModel()
        {
            SyncGames();
        }

        private void SyncGames()
        {
            var games = Directory.GetDirectories(PathHelper.GetDirectoryGamesPath)
                .Select(x => new Game(Path.GetFileName(x)));
            foreach (var game in games)
            {
                _games.Add(game);
            }
        }
    }
}