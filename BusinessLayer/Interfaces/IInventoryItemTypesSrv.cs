namespace BusinessLayer.Interfaces
{
    public interface IInventoryItemTypesSrv
    {
        Task UpdateInventoryItemTypesAsync(Guid inventoryId, IEnumerable<string> itemNames);
    }
}
