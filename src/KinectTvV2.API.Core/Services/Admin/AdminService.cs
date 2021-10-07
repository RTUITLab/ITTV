using System.IO;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Models.S3;
using KinectTvV2.API.Core.Providers.S3;

namespace KinectTvV2.API.Core.Services.Admin
{
    public class AdminService
    {
        private readonly IS3Provider _s3Provider;
        public AdminService(IS3Provider s3Provider)
        {
            _s3Provider = s3Provider;
        }

        public async Task UploadFileAsync(Stream fileStream, string fileName, string directoryName = null)
        {
            await _s3Provider.UploadFileAsync(fileStream, fileName, directoryName);
            //TODO: добавить запись в бд о добавлении файла
            //TODO: вызвать уведомление по SignalR
        }
        public async Task<S3FileInfo> ReadFileAsync(string fileName, string directoryName = null)
        {
            var file = await _s3Provider.ReadFileAsync(fileName, directoryName);
            return file;
        }
    }
}