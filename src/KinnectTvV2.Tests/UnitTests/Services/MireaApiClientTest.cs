using System.Linq;
using System.Threading.Tasks;
using KinectTvV2.Core.Services.ApiClient;
using Xunit;

namespace KinnectTvV2.Tests.UnitTests.Services
{
    public class MireaApiClientTest
    {
        private readonly IMireaApiClient _mireaApiClient;
        
        public MireaApiClientTest()
        {
            _mireaApiClient = new MireaApiClient();
        }

        [Fact]
        public async Task GetNews()
        {
            var actualNews = await _mireaApiClient.GetNews();
            
            Assert.NotEmpty(actualNews);
            
            Assert.NotNull(actualNews.First().Title);
            Assert.NotNull(actualNews.First().Content);

        }

        [Fact]
        public async Task GetScheduler()
        {
            
        }
    }
}