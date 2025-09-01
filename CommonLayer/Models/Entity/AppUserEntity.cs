using CommonLayer.Extensions;
using CommonLayer.Models.Dto.User;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Entity
{
    public class AppUserEntity : IdentityUser<Guid>
    {
        public required string Name { get; set; }
        public required string NormalizedName { get; set; }
        public bool IsBlocked { get; set; }

        public List<InventoryEditorsEntity>? UserInventories { get; set; }

        public AppUserEntity() { }

        [SetsRequiredMembers]
        public AppUserEntity(RegistrationDto dto, string normalizeName, string normalizeEmail)
        {
            Id = Guid.NewGuid();
            Name = $"{dto.LastName} {dto.FirstName}";
            NormalizedName = $"{dto.LastName.CustomNormalize()} {dto.FirstName.CustomNormalize()}";
            UserName = dto.UserName;
            Email = dto.Email;
            AccessFailedCount = 0;
            NormalizedUserName = normalizeName;
            NormalizedEmail = normalizeEmail;
            EmailConfirmed = false;
            ConcurrencyStamp = Guid.NewGuid().ToString();
            PhoneNumber = string.Empty;
            PhoneNumberConfirmed = true;
            TwoFactorEnabled = false;
            IsBlocked = false;
            LockoutEnabled = false;
            LockoutEnd = null;
        }
    }
}
