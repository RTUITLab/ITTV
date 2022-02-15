using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.DTOs;

namespace ITTV.WPF.MVVM.ViewModels.News
{
    public class NewsViewModel : ViewModelBase
    {
        private readonly MireaApiProvider _mireaApiProvider;
        private readonly NavigationStore _navigationStore;
        
        private ObservableCollection<NewsElementViewModel> _news = new();
        public ObservableCollection<NewsElementViewModel> News
        {
            get => _news;
            set
            {
                if (_news.SequenceEqual(value))
                    return;

                _news = value;
                
                OnPropertyChanged(nameof(News));
                OnPropertyChanged(nameof(HasNews));
            }
        }

        public bool HasNews => _news.Count > 0;
        
        public NewsViewModel(MireaApiProvider mireaApiProvider, 
            NavigationStore navigationStore)
        {
            _mireaApiProvider = mireaApiProvider;
            _navigationStore = navigationStore;
        }

        public override async Task Recalculate()
        {
            SetUnloaded();
            
            var data = await _mireaApiProvider.GetNews();
            
            var newsDto = data.Select(x => 
                new NewsDto(x.Title, x.Content, x.Photos.Select(i => i.Data)
                    .ToList()));
            var newsElements = newsDto.Select(x => 
                new NewsElementViewModel(x, _navigationStore));

            News = new ObservableCollection<NewsElementViewModel>(newsElements);
            SetLoaded();
        }
    }
}