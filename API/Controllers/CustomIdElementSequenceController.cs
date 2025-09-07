using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.CustomId;
using CommonLayer.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomIdElementSequenceController : ControllerBase
    {
        private readonly ICustomIdElementSequenceSrv _customIdElementSequenceSrv;
        private readonly ICheckSrv _checkSrv;

        public CustomIdElementSequenceController(
            ICustomIdElementSequenceSrv customIdElementSequenceSrv,
            ICheckSrv checkSrv)
        {
            _customIdElementSequenceSrv = customIdElementSequenceSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("modify")]
        [Authorize]
        public async Task<ActionResult<ResultDto?>> ModifyInventoryItemsRangeAsync([FromBody] CustomIdModifyDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var resultDto = new ResultDto(false, "You are blocked");
                return BadRequest(resultDto);
            }

            if (!(await _checkSrv.IsInventoryCreatorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), [dto.InventoryId])
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory");
                return Ok(checkResult);
            }

            await _customIdElementSequenceSrv.UpdateCustomIdSequenceAsync(dto.InventoryId, dto.ItemId, dto.Sequence);

            return Ok(null);
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CustomIdElementDto>>> GetItemSequenceAsync([FromQuery] Guid inventoryId, [FromQuery] Guid itemId)
        {
            var result = await _customIdElementSequenceSrv.GetItemSequenceAsync(inventoryId, itemId);

            return Ok(result);
        }
    }
}
