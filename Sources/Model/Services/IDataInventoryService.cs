using System.Net;
using Model.Inventory;

namespace Model.Services;

/// <summary>
/// This interface is used to define the methods that will be used to interact with the inventory.
/// </summary>
public interface IDataInventoryService
{
    /// <summary>
    /// Get the inventory.
    /// </summary>
    /// <returns>The inventory.</returns>
    Task<List<InventoryModel>> GetInventory();
    
    /// <summary>
    /// Add an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>The async task.</returns>
    Task AddToInventory(InventoryModel item);

    /// <summary>
    /// Delete an item from the inventory.
    /// </summary>
    /// <param name="position">The position of the item to delete.</param>
    /// <returns>The async task.</returns>
    Task<HttpStatusCode> DeleteFromInventory(int position);

    /// <summary>
    /// Update the inventory.
    /// </summary>
    /// <param name="item">The item to update.</param>
    /// <returns>The async task.</returns>
    Task<HttpStatusCode> UpdateInventory(InventoryModel item);
}
