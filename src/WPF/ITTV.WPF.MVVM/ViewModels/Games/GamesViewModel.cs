﻿using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.Games
{
    public class GamesViewModel : ViewModelBase
    {
        private ObservableCollection<GameDto> _games = new();

        public ObservableCollection<GameDto> Games
        {
            get => _games;
            set
            {
                if (Equals(_games, value))
                    return;

                if (_games != null && value != null)
                {
                    if (_games.SequenceEqual(value))
                        return;
                }

                _games = value;
                OnPropertyChanged(nameof(Games));
            }
        }

        public GamesViewModel()
        { }

        public override void Recalculate()
        {
            SetUnloaded();
            
            var games = Directory.GetDirectories(PathHelper.GetDirectoryGamesPath)
                .Select(x => new GameDto(Path.GetFileName(x)));

            Games = new ObservableCollection<GameDto>(games);
            SetLoaded();
        }
    }
}