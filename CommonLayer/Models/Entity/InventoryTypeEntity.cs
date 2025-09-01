using CommonLayer.Models.Entity.General;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Entity
{
    public class InventoryTypeEntity : IdAndName
    {
        public List<InventoryEntity>? Inventories { get; set; }

        [SetsRequiredMembers]
        public InventoryTypeEntity(string name) : base(name) { }
    }
}
