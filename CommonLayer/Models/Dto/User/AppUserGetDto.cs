using CommonLayer.Models.Entity;

namespace CommonLayer.Models.Dto.User
{
    public class AppUserGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public string ConcurrencyStamp { get; set; }

        public AppUserGetDto() { }

        public AppUserGetDto(AppUserEntity user, bool isAdmin)
        {
            Id = user.Id;
            Name = $"{user.Name}";
            UserName = user.UserName!;
            Email = user.Email!;
            IsBlocked = user.IsBlocked;
            IsAdmin = isAdmin;
            ConcurrencyStamp = user.ConcurrencyStamp;
        }
    }
}
