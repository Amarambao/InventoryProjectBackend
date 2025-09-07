using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTagController : ControllerBase
    {
        private readonly IInventoryTagSrv _invTagSrv;
        private readonly ICheckSrv _checkSrv;

        public InventoryTagController(IInventoryTagSrv invTagSrv,
            ICheckSrv checkSrv)
        {
            _invTagSrv = invTagSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("modify")]
        [Authorize]
        public async Task<ActionResult<ResultDto?>> GetWPaginationAsync([FromBody] IdAndListDto<string> dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var resultDto = new ResultDto(false, "You are blocked");
                return BadRequest(resultDto);
            }

            if (!(await _checkSrv.IsInventoryCreatorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), [dto.Id])
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory tags");
                return Ok(checkResult);
            }

            await _invTagSrv.UpdateInventoryTagsAsync(dto.Id, dto.Values);

            return Ok(null);
        }
    }
}
