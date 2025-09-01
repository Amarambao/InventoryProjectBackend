namespace BusinessLayer.Interfaces
{
    public interface IInventoryEditorsSrv
    {
        Task AddRange(Guid inventoryId, IEnumerable<Guid> userIds);
        Task RemoveRangeAsync(Guid inventoryId, IEnumerable<Guid> userIds);
    }
}
