using System;
using System.Threading.Tasks;
using Amazon.S3;
using KinectTvV2.API.Infrastructure.Data;
using KinectTvV2.API.Requests.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KinectTvV2.API.Controllers
{
    [Route("api/ittv/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAmazonS3 _amazonS3;
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