using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagSrv _tagSrv;

        public TagController(ITagSrv tagSrv)
        {
            _tagSrv = tagSrv;
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetWPaginationAsync([FromQuery] PaginationRequest dto)
        {
            var result = await _tagSrv.GetNamesWithPaginationAsync(dto);

            return Ok(result);
        }
    }
}
