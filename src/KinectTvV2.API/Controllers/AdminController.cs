using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using KinectTvV2.API.Infrastructure.Data;
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
        private readonly IAwsS3HelperService  _amazonS3;
        private readonly ApplicationDbContext _dbContext;
        public AdminController(IAmazonS3 amazonS3, ILogger<AdminController> logger)
        {
            _amazonS3 = amazonS3;
            _logger = logger;
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
        public async Task<IActionResult> UploadNewFile([FromBody] IFormFile request)
        {
            try
            {
                if (request.Length == 0)
                {
                    return BadRequest("please provide valid file");
                }
                var fileName = ContentDispositionHeaderValue
                    .Parse(request.ContentDisposition)
                    .FileName
                    .TrimStart().ToString();
                var folderName = Request.Form.ContainsKey("folder") ? Request.Form["folder"].ToString() : null;
                bool status;
                using (var fileStream = request.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    await fileStream.CopyToAsync(ms);
                    status = await _amazonS3.UploadFileAsync(ms, fileName, folderName);
                }
                return status ? Ok("success")
                    : StatusCode((int)HttpStatusCode.InternalServerError, $"error uploading {fileName}");

                await using var stream = request.OpenReadStream();
                await _amazonS3.UploadObjectFromStreamAsync("bucket", "key", stream, null);
                //TODO: добавить запись о файле
                //TODO: сохранить файл
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't add new file for TV!");
                throw;
            }
        }
    }
} 