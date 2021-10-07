using System.IO;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Models.S3;

namespace KinectTvV2.API.Core.Providers.S3
{
    public interface IS3Provider
    {
        Task<S3FileInfo> ReadFileAsync(string fileName, string directory = null);
        Task UploadFileAsync(Stream fileStream, string fileName, string directory = null);
    }
}