using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        private readonly ICheckSrv _checkSrv;

        public CheckController(ICheckSrv checkSrv)
        {
            _checkSrv = checkSrv;
        }

        [HttpGet("is-admin-check")]
        [Authorize]
        public async Task<ActionResult<bool>> IsAdminAsync([FromQuery] Guid inventoryId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (await _checkSrv.CheckUserStatus(userId))
            {
                var err = new ResultDto(false, "You are blocked");
                return BadRequest(err);
            }

            return Ok(User.IsInRole("admin"));
        }

        [HttpGet("is-inv-creator-check")]
        [Authorize]
        public async Task<ActionResult<bool>> IsInventoryCreatorAsync([FromQuery] Guid inventoryId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (await _checkSrv.CheckUserStatus(userId))
            {
                var err = new ResultDto(false, "You are blocked");
                return BadRequest(err);
            }

            var result = User.IsInRole("admin") || await _checkSrv.IsInventoryCreatorAsync(userId, [inventoryId]);

            return Ok(result);
        }

        [HttpGet("is-editor-check")]
        [Authorize]
        public async Task<ActionResult<bool>> IsUserInventoryEditorAsync([FromQuery] Guid inventoryId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (await _checkSrv.CheckUserStatus(userId))
            {
                var err = new ResultDto(false, "You are blocked");
                return BadRequest(err);
            }

            var result = User.IsInRole("admin") || await _checkSrv.IsUserInventoryEditorAsync(userId, inventoryId);

            return Ok(result);
        }
    }
}
