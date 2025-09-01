using CommonLayer.Models.Entity.General;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Entity
{
    public class ItemTypeEntity : IdAndName
    {
        public List<InventoryItemTypesEntity>? InventoryItemTypes { get; set; }

        [SetsRequiredMembers]
        public ItemTypeEntity(string name) : base(name) { }
    }
}
