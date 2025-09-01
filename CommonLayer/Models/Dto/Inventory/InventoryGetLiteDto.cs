using CommonLayer.Models.Entity;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Dto.Inventory
{
    public class InventoryGetLiteDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required Guid CreatorId { get; set; }
        public required string CreatorName { get; set; }
        public required int ItemsCount { get; set; }

        public InventoryGetLiteDto() { }

        [SetsRequiredMembers]
        public InventoryGetLiteDto(InventoryEntity item)
        {
            var creator = item.InventoryEditors.FirstOrDefault(ui => ui.IsCreator)!.User;

            Id = item.Id;
            Name = item.InventoryType.Name;
            CreatorId = creator.Id;
            CreatorName = creator.UserName ?? string.Empty;
            ItemsCount = item.InventoryItemTypes.SelectMany(ii => ii.StoredItems).Count();
        }
    }
}
