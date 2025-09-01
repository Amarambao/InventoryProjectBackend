namespace CommonLayer.Models.Dto.User
{
    public class ChangeUsersStatusDto
    {
        public bool RequestedStatus { get; set; }
        public required IEnumerable<Guid> UserIds { get; set; }
        public string? RoleName { get; set; }
    }
}
