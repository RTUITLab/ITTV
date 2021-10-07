using System;
using System.IO;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Hubs;
using KinectTvV2.API.Core.Models.S3;
using KinectTvV2.API.Core.Providers.S3;

namespace KinectTvV2.API.Core.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IS3Provider _s3Provider;
        private readonly KinectTvHub _kinectTvHub;
        public AdminService(IS3Provider s3Provider,
            KinectTvHub kinectTvHub)
        {
            _s3Provider = s3Provider;
            _kinectTvHub = kinectTvHub;
        }

        public async Task UploadFileAsync(Stream fileStream, string fileName, string directoryName = null)
        {
            await _s3Provider.UploadFileAsync(fileStream, fileName, directoryName);
            //TODO: добавить запись в бд о добавлении файла
            await _kinectTvHub.VideoUploaded(fileName);
        }
        public async Task<S3FileInfo> ReadFileAsync(string fileName, string directoryName = null)
        {
            var file = await _s3Provider.ReadFileAsync(fileName, directoryName);
            return file;
        }

        public async Task SetDisplayMessage(string displayMessage)
        {
            //TODO: обновление дисплей сообщения
            await _kinectTvHub.SettingsUpdated();
        }

        public async Task SetActiveTime(TimeSpan timeFrom, TimeSpan timeTo)
        {
            //TODO: обновление времени активности
            await _kinectTvHub.SettingsUpdated();
        }

        public async Task Restart()
        {
            await _kinectTvHub.Restart();
        }
    }
}