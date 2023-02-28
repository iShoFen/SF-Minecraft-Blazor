using System.Net;

namespace Model.Services;

/// <summary>
/// This interface is used to define the methods that will be used to interact with the item list.
/// </summary>
public interface IDataItemListService
{
    /// <summary>
    /// Add an item to the item list.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>The async task.</returns>
    Task Add(Item.Item item);
    
    /// <summary>
    /// Delete an item from the item list.
    /// </summary>
    /// <param name="id">The id of the item to delete.</param>
    /// <returns>The async task.</returns>
    Task<HttpStatusCode> Delete(int id);

    /// <summary>
    /// Update an item in the item list.
    /// </summary>
    /// <param name="id">The id of the item to update.</param>
    /// <param name="item">The item to update.</param>
    /// <returns>The async task.</returns>
    Task<HttpStatusCode> Update(int id, Item.Item item);

    /// <summary>
    /// Get all items
    /// </summary>
    /// <returns>The list of items.</returns>
    Task<IEnumerable<Item.Item>> All();
    
    /// <summary>
    /// Get a items with pagination.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>The list of items.</returns>
    Task<IEnumerable<Item.Item>> List(int page, int pageSize);
    
    /// <summary>
    /// Get the number of items in the item list.
    /// </summary>
    /// <returns>The number of items.</returns>
    Task<int> Count();
    
    /// <summary>
    /// Get an item from the item list by id.
    /// </summary>
    /// <param name="id">The id of the item to get.</param>
    /// <returns>The item.</returns>
    Task<Item.Item> GetById(int id);

    /// <summary>
    /// Get an item from the item list by name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <returns>The item.</returns>
    Task<Item.Item> GetByName(string name);

    /// <summary>
    /// Reset the item list.
    /// </summary>
    /// <returns>The async task.</returns>
    Task ResetItems();
}
