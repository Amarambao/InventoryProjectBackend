namespace DataLayer.Interfaces
{
    public interface ICheckRepo
    {
        Task<List<Guid>> GetWhereInventoryCreatorAsync(Guid userId, IEnumerable<Guid> inventoryIds);
        Task<bool> GetWhereInventoryEditorAsync(Guid userId, Guid inventoryId);
    }
}
