using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;
using Serilog;

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

        private readonly NotificationStore _notificationStore;

        public GamesViewModel(NotificationStore notificationStore)
        {
            _notificationStore = notificationStore;
        }

        public override void Recalculate()
        {
            try
            {
                SetUnloaded();

                var games = Directory.GetDirectories(PathHelper.GetDirectoryGamesPath)
                    .Select(x => new GameDto(Path.GetFileName(x)));

                Games = new ObservableCollection<GameDto>(games);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing games");

                var textException = e.InnerException?.Message ?? e.Message;
                _notificationStore.AddNotification(textException);
            }
            finally
            {
                SetLoaded();
            }
        }
    }
}