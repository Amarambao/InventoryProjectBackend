using CommonLayer.Models.Entity;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Dto.Inventory
{
    public class InventoryGetFullDto
    {
        public Guid Id { get; set; }
        public bool IsPublic { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CreatorId { get; set; }
        public required string CreatorName { get; set; }
        public required string ConcurrencyStamp { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public InventoryGetFullDto() { }

        [SetsRequiredMembers]
        public InventoryGetFullDto(InventoryEntity inventory)
        {
            var creator = inventory.InventoryEditors.FirstOrDefault(ui => ui.IsCreator)!.User;

            Id = inventory.Id;
            IsPublic = inventory.IsPublic;
            Name = inventory.InventoryType.Name;
            Description = inventory.Description;
            CreatorId = creator.Id;
            CreatorName = $"{creator.Name}";
            ConcurrencyStamp = Convert.ToBase64String(inventory.ConcurrencyStamp);
            Tags = inventory.InventoryTags.Select(t => t.Tag.Name);
        }
    }
}
