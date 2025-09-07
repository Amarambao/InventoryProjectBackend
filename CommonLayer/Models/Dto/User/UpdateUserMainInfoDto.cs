namespace CommonLayer.Models.Dto.User
{
    public class UpdateUserMainInfoDto
    {
        public Guid Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
