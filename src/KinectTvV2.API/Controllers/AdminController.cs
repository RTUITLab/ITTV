using System;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Helpers;
using KinectTvV2.API.Core.Services.Admin;
using KinectTvV2.API.Requests.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace KinectTvV2.API.Controllers
{
    [Route("api/ittv/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;
        public AdminController(ILogger<AdminController> logger, 
            IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        #region TV
        [HttpGet]
        public async Task<IActionResult> Restart()
        {
            try
            {
                await _adminService.Restart();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't restart service!");
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetActiveTime([FromBody] ApiSetActiveTimeRequest request)
        {
            try
            {
                await _adminService.SetActiveTime(request.TimeFrom.GetTimeSpan, request.TimeTo.GetTimeSpan);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't set active time for TV!");
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetDisplayMessage([FromBody] ApiSetDisplayMessageRequest request)
        {
            try
            {
                await _adminService.SetDisplayMessage(request.Message);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't set display message for TV!");
                return BadRequest(e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTvConfiguration()
        {
            try
            {
                var result = await _adminService.GetTvConfiguration();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't take TV Configuration!");
                return BadRequest(e);
            }
        }
        #endregion
        
        #region S3
        [HttpPost]
        public async Task<IActionResult> UploadNewFile([FromForm] IFormFile file)
        {
            try
            {
                if (file.Length == 0)
                {
                    return BadRequest("Please provide valid file!");
                }
                
                var fileName = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .TrimStart().ToString();

                var baseFileName = Base64Helper.Encode(fileName);
                
                //TODO: добавить поддержку directory
                await using var fileStream = file.OpenReadStream();
                await _adminService.UploadFileAsync(fileStream, baseFileName);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't add new file for TV!");
                return BadRequest(e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFile([FromQuery] string baseFileName)
        {
            try
            {
                var fileName = Base64Helper.Decode(baseFileName);
                var file = await _adminService.ReadFileAsync(baseFileName);

                var result = File(file.FileData, file.ContentType, fileName);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't get file with baseName {0}", baseFileName);
                return BadRequest(e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListFiles([FromQuery] DateTime? createdFrom = null)
        {
            try
            {
                return Ok(await _adminService.GetFileList(createdFrom));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't get list files from {0}",createdFrom);
                return BadRequest(e);
            }
        }
        #endregion
        
    }
} 