using System.IO;
using System.Threading.Tasks;
using ITTV.API.Core.Models.S3;

namespace ITTV.API.Core.Providers.S3
{
    public interface IS3Provider
    {
        Task<S3FileInfo> ReadFileAsync(string fileName, string directory = null);
        Task UploadFileAsync(Stream fileStream, string fileName, string directory = null);
    }
}