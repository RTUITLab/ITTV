using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace KinectTvV2.API.Core.Hubs.KinectTvHub
{
    public class KinectTvHubHandler : IKinectTvHubHandler
    {
        private readonly IHubContext<KinectTvHub> _hubContext;

        public KinectTvHubHandler(IHubContext<KinectTvHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Restart()
        {
            await _hubContext.Clients.All.SendAsync(nameof(Restart));
        }
        public async Task VideoUploaded(string baseFileName)
        {
            await _hubContext.Clients.All.SendAsync(nameof(VideoUploaded), baseFileName);
        }        
        public async Task SettingsUpdated()
        {
            await _hubContext.Clients.All.SendAsync(nameof(SettingsUpdated));
        }
    }
}