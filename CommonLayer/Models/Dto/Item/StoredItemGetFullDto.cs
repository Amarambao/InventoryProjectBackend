using CommonLayer.Models.Dto.CustomDescription;
using CommonLayer.Models.Entity;

namespace CommonLayer.Models.Dto.Item
{
    public class StoredItemGetFullDto
    {
        public Guid Id { get; set; }
        public string CustomId { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ItemDescriptionElementDto> Description { get; set; }

        public StoredItemGetFullDto() { }

        public StoredItemGetFullDto(StoredItemsEntity item)
        {
            Id = item.Id;
            CustomId = item.CustomId;
            CreatorId = item.CreatorId;
            CreatorName = item.CreatorName;
            CreatedAt = item.CreatedAt;
            Description = item.StoredItemDescriptions.OrderBy(sid => sid.Order).Select(sid => new ItemDescriptionElementDto(sid)).ToList();
        }
    }
}
