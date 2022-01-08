using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class NewsViewModel : ViewModelBase
    {
        private readonly MireaApiProvider _mireaApiProvider;
        private readonly NavigationStore _navigationStore;
        
        private readonly ObservableCollection<NewsElementViewModel> _news = new();
        public ObservableCollection<NewsElementViewModel> News => _news;
        
        public NewsViewModel(MireaApiProvider mireaApiProvider, 
            NavigationStore navigationStore)
        {
            _mireaApiProvider = mireaApiProvider;
            _navigationStore = navigationStore;

            SyncNews().ConfigureAwait(false);
        }

        private async Task SyncNews()
        {
            var data = await _mireaApiProvider.GetNews();
            
            var newsDto = data.Select(x => 
                new NewsDto(x.Title, x.Content, x.Photos.Select(i => i.Data)
                    .ToList()));
            var newsElements = newsDto.Select(x => 
                new NewsElementViewModel(x, _navigationStore));

            foreach (var newsElement in newsElements)
            {
                _news.Add(newsElement);
            }
        }
    }
}