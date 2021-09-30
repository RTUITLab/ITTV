using System;
using System.Threading.Tasks;
using KinectTvV2.API.Requests.Admin;
using Microsoft.AspNetCore.Mvc;

namespace KinectTvV2.API.Controllers
{
    [Route("api/ittv/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public AdminController()
        {

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
                Console.WriteLine(e);
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
                Console.WriteLine(e);
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
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
    }
}