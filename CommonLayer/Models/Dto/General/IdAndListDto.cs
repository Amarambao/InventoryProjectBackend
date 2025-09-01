namespace CommonLayer.Models.Dto.General
{
    public class IdAndListDto<T>
    {
        public Guid Id { get; set; }
        public List<T> Values { get; set; }
    }
}
