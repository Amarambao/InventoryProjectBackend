namespace CommonLayer.Models.Dto.User
{
    public class LoginDto
    {
        public required string EmailOrUserName { get; set; }
        public required string Password { get; set; }
    }
}
