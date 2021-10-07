using System.IO;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Models.S3;

namespace KinectTvV2.API.Core.Services.Admin
{
    public interface IAdminService
    {
        Task UploadFileAsync(Stream fileStream, string fileName, string directoryName = null);
        Task<S3FileInfo> ReadFileAsync(string fileName, string directoryName = null);
    }
}