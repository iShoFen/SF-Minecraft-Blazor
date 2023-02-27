// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryController.cs" company="UCA Clermont-Ferrand">
//     Copyright (c) UCA Clermont-Ferrand All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Model;
namespace RestController.Controllers;

/// <summary>
/// The inventory controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private const string InventoryPath = "Data/inventory.json";
    private const string InventoryNotFoundMessage = "Unable to get the inventory.";
    private const string ItemNotFoundMessage = "Unable to found the item with id: {0} at position: {1}";
    
    /// <summary>
    /// The json serializer options.
    /// </summary>
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    /// <summary>
    /// Adds to inventory.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The async task.</returns>
    [HttpPost]
    [Route("")]
    public Task AddToInventory(InventoryModel item)
    {
        var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText(InventoryPath), _jsonSerializerOptions);

        if (data == null)
        {
            throw new FileNotFoundException(InventoryNotFoundMessage);
        }

        data.Add(item);

        System.IO.File.WriteAllText(InventoryPath, JsonSerializer.Serialize(data, _jsonSerializerOptions));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes from inventory.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The async task.</returns>
    [HttpDelete]
    [Route("")]
    public Task DeleteFromInventory(InventoryModel item)
    {
        var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText(InventoryPath), _jsonSerializerOptions);

        if (data == null)
        {
            throw new FileNotFoundException(InventoryNotFoundMessage);
        }

        var inventoryItem = data.FirstOrDefault(w => w.ItemId == item.ItemId && w.Position == item.Position);

        if (inventoryItem == null)
        {
            throw new ArgumentException(string.Format(ItemNotFoundMessage, item.ItemId, item.Position));
        }

        data.Remove(inventoryItem);

        System.IO.File.WriteAllText(InventoryPath, JsonSerializer.Serialize(data, _jsonSerializerOptions));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the inventory.
    /// </summary>
    /// <returns>The inventory.</returns>
    [HttpGet]
    [Route("")]
    public Task<List<InventoryModel>> GetInventory()
    {
        if (!System.IO.File.Exists(InventoryPath))
        {
            System.IO.File.Create(InventoryPath).Close();
            return Task.FromResult(new List<InventoryModel>());
        }

        var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText(InventoryPath), _jsonSerializerOptions);
            
        return Task.FromResult(data!);
    }

    /// <summary>
    /// Updates the inventory.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>The async task.</returns>
    [HttpPut]
    [Route("")]
    public Task UpdateInventory(InventoryModel item)
    {
        var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText(InventoryPath), _jsonSerializerOptions);

        if (data == null)
        {
            throw new FileNotFoundException(InventoryNotFoundMessage);
        }

        var inventoryItem = data.FirstOrDefault(w => w.ItemId == item.ItemId && w.Position == item.Position);

        if (inventoryItem == null)
        {
            throw new ArgumentException(string.Format(ItemNotFoundMessage, item.ItemId, item.Position));
        }

        inventoryItem.ItemId = item.ItemId;
        inventoryItem.Position = item.Position;

        System.IO.File.WriteAllText(InventoryPath, JsonSerializer.Serialize(data, _jsonSerializerOptions));

        return Task.CompletedTask;
    }
}