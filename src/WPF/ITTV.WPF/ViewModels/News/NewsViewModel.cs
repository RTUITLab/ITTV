﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.DTOs;
using Serilog;

namespace ITTV.WPF.ViewModels.News
{
    public class NewsViewModel : ViewModelBase
    {
        private readonly MireaApiProvider _mireaApiProvider;
        private readonly NavigationStore _navigationStore;
        private readonly NotificationStore _notificationStore;
        
        private ObservableCollection<NewsElementViewModel> _news = new();
        public ObservableCollection<NewsElementViewModel> News
        {
            get => _news;
            set
            {
                if (Equals(_news, value))
                    return;
                
                if (_news != null && value != null)
                {
                    if (_news.SequenceEqual(value))
                        return;
                }

                _news = value;
                
                OnPropertyChanged(nameof(News));
                OnPropertyChanged(nameof(HasNews));
            }
        }

        public bool HasNews => _news.Count > 0;
        
        public NewsViewModel(MireaApiProvider mireaApiProvider, 
            NavigationStore navigationStore, 
            NotificationStore notificationStore)
        {
            _mireaApiProvider = mireaApiProvider;
            _navigationStore = navigationStore;
            _notificationStore = notificationStore;
        }

        public override async void Recalculate()
        {
            try
            {
                SetUnloaded();

                var data = await _mireaApiProvider.GetNews();

                var newsDto = data.Select(x =>
                    new NewsDto(x.Title, x.Content, x.Photos.Select(i => i.Data)
                        .ToList()));
                var newsElements = newsDto.Select(x =>
                    new NewsElementViewModel(x, _navigationStore));

                News = new ObservableCollection<NewsElementViewModel>(newsElements);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Exception while syncing news");

                _notificationStore.AddNotification(e);
            }
            finally
            {
                SetLoaded();
            }
        }
    }
}