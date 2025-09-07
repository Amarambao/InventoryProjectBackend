using CommonLayer.Enum;
using CommonLayer.Models.Dto.CustomDescription;

namespace CommonLayer.Models.Entity
{
    public class StoredItemDescriptionEntity
    {
        public Guid Id { get; set; }
        public Guid StoredItemId { get; set; }
        public CustomDescriptionFieldEnum DescriptionType { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string? ShortText { get; set; }
        public string? LongText { get; set; }
        public string? Href { get; set; }
        public int? Number { get; set; }
        public bool? Bool { get; set; }

        public StoredItemsEntity StoredItem { get; set; }

        public StoredItemDescriptionEntity() { }

        public StoredItemDescriptionEntity(StoredItemsEntity item, ItemDescriptionElementDto itemDescription, int order, string name)
        {
            Id = Guid.NewGuid();
            StoredItemId = item.Id;
            DescriptionType = itemDescription.DescriptionType;
            Name = name;
            Order = order;
            ShortText = itemDescription.DescriptionType == CustomDescriptionFieldEnum.SingleLineText ? itemDescription.ShortTextValue : null;
            LongText = itemDescription.DescriptionType == CustomDescriptionFieldEnum.MultiLineText ? itemDescription.LongTextValue : null;
            Href = itemDescription.DescriptionType == CustomDescriptionFieldEnum.DocumentLink ? itemDescription.LongTextValue : null;
            Number = itemDescription.DescriptionType == CustomDescriptionFieldEnum.Numeric ? itemDescription.NumberValue : null;
            Bool = itemDescription.DescriptionType == CustomDescriptionFieldEnum.BooleanValue ? itemDescription.BoolValue : null;
        }
    }
}
