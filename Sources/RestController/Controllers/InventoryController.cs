// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryController.cs" company="UCA Clermont-Ferrand">
//     Copyright (c) UCA Clermont-Ferrand All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Inventory;

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
    private const string ItemNotFoundMessage = "Unable to found the item at position: {0}";
    
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
        List<InventoryModel>? data;
        data = !System.IO.File.Exists(InventoryPath) 
            ? new List<InventoryModel>() 
            : JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText(InventoryPath), _jsonSerializerOptions);
        
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
    /// <param name="position">The position.</param>
    /// <returns>The async task.</returns>
    [HttpDelete]
    [Route("{position:int}")]
    public Task DeleteFromInventory(int position)
    {
        var data = JsonSerializer.Deserialize<List<InventoryModel>>(System.IO.File.ReadAllText(InventoryPath), _jsonSerializerOptions);

        if (data == null)
        {
            throw new FileNotFoundException(InventoryNotFoundMessage);
        }

        var inventoryItem = data.FirstOrDefault(w => w.Position == position);

        if (inventoryItem == null)
        {
            throw new ArgumentException(string.Format(ItemNotFoundMessage, position));
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
        if (!System.IO.File.Exists(InventoryPath))  return Task.FromResult(new List<InventoryModel>());

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
            throw new ArgumentException(string.Format(ItemNotFoundMessage, item.Position));
        }
        
        inventoryItem.NumberItem = item.NumberItem;

        System.IO.File.WriteAllText(InventoryPath, JsonSerializer.Serialize(data, _jsonSerializerOptions));

        return Task.CompletedTask;
    }
}