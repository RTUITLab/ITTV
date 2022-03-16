using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ITTV.API.Core.Hubs.KinectTvHub
{
    public class KinectTvHub : Hub
    {
        public async Task SetTvStatus()
        {
           //TODO: задание статуса TV, online&offline
        }
    }
}