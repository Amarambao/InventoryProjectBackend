namespace BusinessLayer.Interfaces
{
    public interface IInventoryItemTypesSrv
    {
        Task ModifyInventoryItemsRangeAsync(Guid inventoryId, IEnumerable<string> itemNames);
    }
}
