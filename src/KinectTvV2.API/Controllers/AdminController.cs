using System;
using System.Threading.Tasks;
using KinectTvV2.API.Core.Services.Admin;
using KinectTvV2.API.Requests.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Restart()
        {
            try
            {
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
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't set display message for TV!");
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadNewFile([FromBody] IFormFile file)
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
                //TODO: добавить поддержку directory
                await using var fileStream = file.OpenReadStream();
                await _adminService.UploadFileAsync(fileStream, fileName);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't add new file for TV!");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFile([FromQuery] string baseFileName)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(baseFileName);
                var fileName = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                //TODO: добавить поддержку directory
                var file = await _adminService.ReadFileAsync(baseFileName);

                var result = File(file.FileData, file.ContentType, fileName);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
} 