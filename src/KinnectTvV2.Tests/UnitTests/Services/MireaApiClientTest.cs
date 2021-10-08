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
            var actualSchedule = await _mireaApiClient.GetFullScheduleForGroup(testGroup);
            
            Assert.NotNull(actualSchedule.FirstWeek);
            Assert.NotNull(actualSchedule.SecondWeek);
        }
        [Fact]
        public async Task GetTodayScheduleForGroup()
        {
            const string testGroup = "ИКБО-24-20";
            var actualSchedule = await _mireaApiClient.GetTodayScheduleForGroup(testGroup);
            
            Assert.NotEmpty(actualSchedule);
            
            Assert.NotNull(actualSchedule.First().DetailLesson);
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