using System.IO;
using System.Threading.Tasks;

namespace KinectTvV2.API.Core.Services.Admin
{
    public interface IAdminService
    {
        Task UploadFileAsync(Stream fileStream, string fileName, string directoryName = null);
    }
}