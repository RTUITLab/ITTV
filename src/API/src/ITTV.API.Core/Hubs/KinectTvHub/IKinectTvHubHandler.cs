using System.Threading.Tasks;

namespace ITTV.API.Core.Hubs.KinectTvHub
{
    public interface IKinectTvHubHandler
    {
        Task Restart();
        Task VideoUploaded(string baseFileName);
        Task SettingsUpdated();
    }
}