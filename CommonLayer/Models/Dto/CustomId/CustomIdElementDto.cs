using CommonLayer.Enum;

namespace CommonLayer.Models.Dto.CustomId
{
    public class CustomIdElementDto
    {
        public CustomIdElementEnum ElementType { get; set; }
        public string? FixedTextValue { get; set; }
        public int? IncrementValue { get; set; }
    }
}
