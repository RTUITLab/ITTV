using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace KinectTvV2.API.Core.Hubs
{
    public class KinectTvHub : Hub
    {
        public async Task Restart()
        {
            await Clients.All.SendAsync(nameof(Restart));
        }
        public async Task VideoUploaded(string baseFileName)
        {
            await Clients.All.SendAsync(nameof(VideoUploaded), baseFileName);
        }        
        public async Task SettingsUpdated()
        {
            await Clients.All.SendAsync(nameof(SettingsUpdated));
        }
    }
}