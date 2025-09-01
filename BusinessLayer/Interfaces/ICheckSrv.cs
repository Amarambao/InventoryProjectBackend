namespace BusinessLayer.Interfaces
{
    public interface ICheckSrv
    {
        Task<bool> CheckUserStatus(Guid userId);
        Task<bool> IsInventoryCreatorAsync(Guid userId, IEnumerable<Guid> inventoryIds);
        Task<bool> IsUserInventoryEditorAsync(Guid userId, Guid inventoryId);
    }
}
