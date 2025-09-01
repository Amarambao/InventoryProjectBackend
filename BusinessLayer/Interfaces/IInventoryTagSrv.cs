namespace BusinessLayer.Interfaces
{
    public interface IInventoryTagSrv
    {
        Task ModifyInventoryTagsRangeAsync(Guid inventoryId, IEnumerable<string> tagsRequest);
    }
}
