using System;
using System.IO;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Helpers;
using KinectTvV2.API.Core.Hubs.KinectTvHub;
using KinectTvV2.API.Core.Models.ITTV;
using KinectTvV2.API.Core.Models.S3;
using KinectTvV2.API.Core.Providers.S3;
using KinectTvV2.API.Domain.Entities;
using KinectTvV2.API.Infrastructure.Data;
using KinectTvV2.Core.Providers.LocalCache;

namespace KinectTvV2.API.Core.Services.Admin
{
    public class AdminService : IAdminService
    {
        private const string ITTVConfigurationPath = "ittv-configuration";
        
        private readonly IS3Provider _s3Provider;
        private readonly IKinectTvHubHandler _kinectTvHubHandler;
        private readonly ApplicationDbContext _dbContext;
        public AdminService(IS3Provider s3Provider,
            IKinectTvHubHandler kinectTvHubHandler, ApplicationDbContext dbContext)
        {
            _s3Provider = s3Provider;
            _kinectTvHubHandler = kinectTvHubHandler;
            _dbContext = dbContext;
        }

        #region S3
        public async Task UploadFileAsync(Stream fileStream, string fileName, string directoryName = null)
        {
            await _s3Provider.UploadFileAsync(fileStream, fileName, directoryName);
            var fileInfo = new FileInfoEntity(fileName);
            
            await _dbContext.AddAsync(fileInfo);
            await _dbContext.SaveChangesAsync();
            
            await _kinectTvHubHandler.VideoUploaded(fileName);
        }
        public async Task<S3FileInfo> ReadFileAsync(string baseFileName, string directoryName = null)
        {
            var fileName = Base64Helper.Decode(baseFileName);
            var file = await _s3Provider.ReadFileAsync(fileName, directoryName);
            return file;
        }
        #endregion

        #region ITTVConfiguration
        public async Task SetDisplayMessage(string displayMessage)
        {
            var configuration = await LocalCacheProvider.GetAsync<ITTVConfiguration>(ITTVConfigurationPath) ?? new ITTVConfiguration();
            configuration.SetDisplayMessage(displayMessage);
            await LocalCacheProvider.PutAsync(ITTVConfigurationPath, configuration);
            await _kinectTvHubHandler.SettingsUpdated();
        }

        public async Task SetActiveTime(TimeSpan timeFrom, TimeSpan timeTo)
        {
            var configuration = await LocalCacheProvider.GetAsync<ITTVConfiguration>(ITTVConfigurationPath) ?? new ITTVConfiguration();
            configuration.SetActiveTime(timeFrom, timeTo);
            await LocalCacheProvider.PutAsync(ITTVConfigurationPath, configuration);
            await _kinectTvHubHandler.SettingsUpdated();
        }

        public async Task<ITTVConfiguration> GetTvConfiguration()
        => await LocalCacheProvider.GetAsync<ITTVConfiguration>(ITTVConfigurationPath) ?? new ITTVConfiguration();
        #endregion
        
        public async Task Restart()
        {
            await _kinectTvHubHandler.Restart();
        }
    }
}