using System.Threading.Tasks;

namespace KinectTvV2.API.Core.Hubs.KinectTvHub
{
    public interface IKinectTvHubHandler
    {
        Task Restart();
        Task VideoUploaded(string baseFileName);
        Task SettingsUpdated();
    }
}