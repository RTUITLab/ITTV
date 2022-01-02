using System.Linq;
using System.Threading.Tasks;
using ITTV.WPF.Core.Services.ApiClient;
using Xunit;

namespace ITTV.WPF.Tests.UnitTests.Services
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
        }

        [Fact]
        public async Task GetAllGroups()
        {
            var actualGroups = await _mireaApiClient.GetAllGroups();
            
            Assert.NotEmpty(actualGroups.Bachelor.First);
            Assert.NotEmpty(actualGroups.Bachelor.Second);
            Assert.NotEmpty(actualGroups.Bachelor.Third);
            Assert.NotEmpty(actualGroups.Bachelor.Fourth);

            Assert.NotEmpty(actualGroups.Master.First);
            Assert.NotEmpty(actualGroups.Master.Second);
        }
    }
}