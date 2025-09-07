namespace BusinessLayer.Interfaces
{
    public interface IInventoryTagSrv
    {
        Task UpdateInventoryTagsAsync(Guid inventoryId, IEnumerable<string> tagsRequest);
    }
}
