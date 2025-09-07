using CommonLayer.Enum;
using CommonLayer.Models.Entity;

namespace CommonLayer.Models.Dto.CustomDescription
{
    public class ItemDescriptionElementDto
    {
        public CustomDescriptionFieldEnum DescriptionType { get; set; }
        public string? ShortTextValue { get; set; }
        public string? LongTextValue { get; set; }
        public string? HlinkValue { get; set; }
        public int? NumberValue { get; set; }
        public bool? BoolValue { get; set; }

        public ItemDescriptionElementDto() { }

        public ItemDescriptionElementDto(StoredItemDescriptionEntity description)
        {
            DescriptionType = description.DescriptionType;
            ShortTextValue = description.DescriptionType == CustomDescriptionFieldEnum.SingleLineText ? description.ShortText : null;
            LongTextValue = description.DescriptionType == CustomDescriptionFieldEnum.DocumentLink ? description.LongText : null;
            HlinkValue = description.DescriptionType == CustomDescriptionFieldEnum.DocumentLink ? description.LongText : null;
            NumberValue = description.DescriptionType == CustomDescriptionFieldEnum.Numeric ? description.Number : null;
            BoolValue = description.DescriptionType == CustomDescriptionFieldEnum.BooleanValue ? description.Bool : null;
        }
    }
}
