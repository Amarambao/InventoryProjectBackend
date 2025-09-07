using CommonLayer.Models.Dto.General;

namespace CommonLayer.Models.Dto.User
{
    public class ChangeUsersStatusDto
    {
        public bool RequestedStatus { get; set; }
        public required IEnumerable<IdAndStringDto> UserIdAndStamp { get; set; }
        public string? RoleName { get; set; }
    }
}
