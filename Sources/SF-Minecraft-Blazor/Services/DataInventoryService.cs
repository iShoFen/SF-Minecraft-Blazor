using System.Net;
using Model.Inventory;
using Model.Services;

namespace SF_Minecraft_Blazor.Services;

public class DataInventoryService: IDataInventoryService
{
    private readonly HttpClient _http;
    
    private readonly ILogger<DataInventoryService> _logger;
     
    public DataInventoryService(HttpClient http, ILogger<DataInventoryService> logger)
    {
        _http = http;
        _logger = logger;
        
        _logger.LogInformation("DataInventoryService created");
    }

    public async Task<List<InventoryModel>> GetInventory()
    {
        var response = await _http.GetFromJsonAsync<List<InventoryModel>>("Inventory/");
        if (response == null)
        {
            _logger.LogWarning("GetInventory returned null");
            return new List<InventoryModel>();
        }
        _logger.LogInformation("{InventoryCount} items retrieved", response.Count);
        
        return response;
    }

    public async Task AddToInventory(InventoryModel item)
    {
        var reponse = await _http.PostAsJsonAsync("Inventory/", item);
        if (reponse.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("AddToInventory failed with {StatusCode}", reponse.StatusCode);
        }
        
        _logger.LogInformation("AddToInventory succeeded with {StatusCode}", reponse.StatusCode);
    }

    public async Task<HttpStatusCode> DeleteFromInventory(int position)
    {
        var response = await _http.DeleteAsync($"Inventory/{position}");
        if (response.StatusCode != HttpStatusCode.OK) _logger.LogWarning("DeleteFromInventory failed with {StatusCode}", response.StatusCode);
        else _logger.LogInformation("DeleteFromInventory succeeded with {StatusCode}", response.StatusCode);
        
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> UpdateInventory(InventoryModel item)
    {
        var response = await _http.PutAsJsonAsync("Inventory/", item);
        if (response.StatusCode != HttpStatusCode.OK) _logger.LogWarning("UpdateInventory failed with {StatusCode}", response.StatusCode);
        else _logger.LogInformation("UpdateInventory succeeded with {StatusCode}", response.StatusCode);
        
        return response.StatusCode;
    }
}
