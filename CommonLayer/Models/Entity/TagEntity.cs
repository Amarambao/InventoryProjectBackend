using CommonLayer.Models.Entity.General;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Entity
{
    public class TagEntity : IdAndName
    {
        public List<InventoryTagEntity>? InventoryTags { get; set; }

        [SetsRequiredMembers]
        public TagEntity(string name) : base(name) { }
    }
}
