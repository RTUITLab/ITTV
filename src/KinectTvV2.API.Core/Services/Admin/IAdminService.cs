using System;
using System.IO;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Models;
using KinectTvV2.API.Core.Models.ITTV;
using KinectTvV2.API.Core.Models.S3;

namespace KinectTvV2.API.Core.Services.Admin
{
    public interface IAdminService
    {
        /// <summary>
        /// Загрузка новых файлов, предполагает уведомление по SignalR об изменении
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="baseFileName">Название файла в формате base64</param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        Task UploadFileAsync(Stream fileStream, string baseFileName, string directoryName = null);
        /// <summary>
        /// Получение файлов из хранилища
        /// </summary>
        /// <param name="baseFileName">Название файла в формате base64</param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        Task<S3FileInfo> ReadFileAsync(string baseFileName, string directoryName = null);
        /// <summary>
        /// Задания выводимого сообщения на телевизоре
        /// </summary>
        /// <param name="displayMessage"></param>
        /// <returns></returns>
        Task SetDisplayMessage(string displayMessage);
        /// <summary>
        /// Задание времени активности телевизора, предполагает уведомление по SignalR об изменении
        /// </summary>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        Task SetActiveTime(TimeSpan timeFrom, TimeSpan timeTo);
        /// <summary>
        /// Получение актуальной конфигурации
        /// </summary>
        /// <returns></returns>
        Task<ITTVConfiguration> GetTvConfiguration();

        /// <summary>
        /// Перезагрузка телевизора, уведомление по SignalR
        /// </summary>
        /// <returns></returns>
        Task Restart();
        /// <summary>
        /// Получение списка файлов, которые были загружен с определенной даты
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        Task<ApiFileInfo[]> GetFileList(DateTime? dateFrom);
    }
}