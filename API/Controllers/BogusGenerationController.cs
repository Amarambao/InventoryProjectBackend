using BusinessLayer.Interfaces.Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
#if DEBUG
    [Route("api/[controller]")]
    [ApiController]
    public class BogusGenerationController : ControllerBase
    {
        private readonly IBogusGenerationSrv _bogusGenerationSrv;

        public BogusGenerationController(IBogusGenerationSrv bogusGenerationSrv)
        {
            _bogusGenerationSrv = bogusGenerationSrv;
        }

        [HttpPost("create-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdminAsync()
        {
            await _bogusGenerationSrv.CreateAdminAsync();
            return Ok();
        }

        [HttpPost("create-main-data")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateMainDataAsync()
        {
            await _bogusGenerationSrv.CreateMainDataAsync();
            return Ok();
        }
    }
#endif
}
