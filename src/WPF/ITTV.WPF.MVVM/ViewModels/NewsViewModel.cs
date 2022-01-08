using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Abstractions.Base.ViewModel;
using ITTV.WPF.Core.Providers.MireaApi;

namespace ITTV.WPF.MVVM.ViewModels
{
    public class NewsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<NewsElementViewModel> _news = new();
        public ObservableCollection<NewsElementViewModel> News => _news;
        
        private readonly MireaApiProvider _mireaApiProvider;
        public NewsViewModel(MireaApiProvider mireaApiProvider)
        {
            _mireaApiProvider = mireaApiProvider;
        }

        public async Task SyncNews()
        {
            var data = await _mireaApiProvider.GetNews();
            
            var newsElements = data.Select(x => 
                new NewsElementViewModel(x.));
        }
    }
}