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
            Assert.NotEmpty(actualNews.First().Photos);
        }

        [Fact]
        public async Task GetScheduleForGroup()
        {
            const string testGroup = "ИКБО-24-20";
            var actualSchedule = await _mireaApiClient.GetScheduleForGroup(testGroup);
            
            Assert.NotNull(actualSchedule.Result);
            
            Assert.NotNull(actualSchedule.Result.Friday);
            Assert.NotNull(actualSchedule.Result.Monday);
            Assert.NotNull(actualSchedule.Result.Saturday);
            Assert.NotNull(actualSchedule.Result.Thursday);
            Assert.NotNull(actualSchedule.Result.Tuesday);
            Assert.NotNull(actualSchedule.Result.Wednesday);
            
            Assert.NotNull(actualSchedule.Result.Monday.Weeks);
        }

        [Fact]
        public async Task GetAllGroups()
        {
            var actualGroups = await _mireaApiClient.GetAllGroups();
            
            Assert.NotEmpty(actualGroups.Groups);
            Assert.NotEqual(0, actualGroups.Count);
        }
    }
}