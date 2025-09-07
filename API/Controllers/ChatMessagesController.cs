using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.ChatMessage;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessagesSrv _chatMessagesSrv;
        private readonly ICheckSrv _checkSrv;

        public ChatMessagesController(
            IChatMessagesSrv chatMessagesSrv,
            ICheckSrv checkSrv)
        {
            _chatMessagesSrv = chatMessagesSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult<ResultDto?>> CreateMessageAsync([FromBody] MessagePostDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (await _checkSrv.CheckUserStatus(userId))
                return BadRequest(new ResultDto(false, "You are blocked"));

            if (!(await _checkSrv.IsUserInventoryEditorAsync(userId, dto.InventoryId)
                || User.IsInRole("admin")))
                return Ok(new ResultDto(false, "You are not allowed to edit inventory"));

            await _chatMessagesSrv.CreateMessageAsync(dto, userId);

            return Ok();
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultDto<IEnumerable<MessageGetDto>>>> GetChatMessagesWPaginationAsync([FromQuery] MessageRequestDto dto)
        {
            var result = await _chatMessagesSrv.GetChatMessagesWPaginationAsync(dto);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult<ResultDto<IEnumerable<MessageGetDto>>>> RemoveMessageRangeAsync([FromQuery] Guid inventoryId, IEnumerable<string> dateTimes)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (await _checkSrv.CheckUserStatus(userId))
                return BadRequest(new ResultDto(false, "You are blocked"));

            if (!(await _checkSrv.IsInventoryCreatorAsync(userId, [inventoryId])
                || User.IsInRole("admin")))
                return Ok(new ResultDto(false, "You are not allowed to edit inventory"));

            await _chatMessagesSrv.RemoveMessageRangeAsync(dateTimes
                .Select(dt => DateTime.TryParse(dt, out var parsed) ? parsed : (DateTime?)null)
                .Where(d => d.HasValue)
                .Select(d => d!.Value.ToUniversalTime()));

            return Ok(null);
        }
    }
}
