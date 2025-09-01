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
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var resultDto = new ResultDto(false, "You are blocked");
                return BadRequest(resultDto);
            }

            if (!(await _checkSrv.IsUserInventoryEditorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), dto.InventoryId)
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory");
                return Ok(checkResult);
            }

            await _chatMessagesSrv.CreateMessageAsync(dto, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

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
        public async Task<ActionResult<ResultDto<IEnumerable<MessageGetDto>>>> RemoveMessageRangeAsync([FromQuery] Guid inventoryId, IEnumerable<DateTime> dateTimes)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var resultDto = new ResultDto(false, "You are blocked");
                return BadRequest(resultDto);
            }

            if (!(await _checkSrv.IsInventoryCreatorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), [inventoryId])
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory");
                return Ok(checkResult);
            }

            await _chatMessagesSrv.RemoveMessageRangeAsync(dateTimes);

            return Ok();
        }
    }
}
