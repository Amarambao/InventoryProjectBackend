using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.CustomDescription;
using CommonLayer.Models.Dto.CustomId;
using CommonLayer.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomDescriptionSequenceController :ControllerBase
    {
        private readonly ICustomDescriptionSequenceSrv _customDescriptionSrv;
        private readonly ICheckSrv _checkSrv;

        public CustomDescriptionSequenceController(
            ICustomDescriptionSequenceSrv customDescriptionSrv,
            ICheckSrv checkSrv)
        {
            _customDescriptionSrv = customDescriptionSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("modify")]
        [Authorize]
        public async Task<ActionResult<ResultDto?>> ModifyCustomDescriptionRangeAsync([FromBody] CustomDescriptionSequencePostDto dto)
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

            await _customDescriptionSrv.ModifyCustomDescriptionSequenceAsync(dto.InventoryId, dto.ItemId, dto.Sequence);

            return Ok(null);
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DescriptionElementDto>>> GetItemSequenceAsync([FromQuery] Guid inventoryId, [FromQuery] Guid itemId)
        {
            var result = await _customDescriptionSrv.GetDescriptionSequenceAsync(inventoryId, itemId);

            return Ok(result);
        }
    }
}
