using System.Threading.Tasks;

namespace KinectTvV2.API.Core.Hubs.KinectTvHub
{
    public interface IKinectTvHubHandler
    {
        public Task Restart();
        public Task VideoUploaded(string baseFileName);
        public Task SettingsUpdated();
    }
}