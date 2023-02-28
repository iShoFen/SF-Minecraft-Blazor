using System.Net;
using Model.Inventory;
using Model.Services;

namespace SF_Minecraft_Blazor.Services;

public class DataInventoryService: IDataInventoryService
{
    private readonly HttpClient _http;
     
    public DataInventoryService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<List<InventoryModel>> GetInventory() 
        => await _http.GetFromJsonAsync<List<InventoryModel>>("Inventory/") ?? new List<InventoryModel>();
    
    public async Task AddToInventory(InventoryModel item) 
        => await _http.PostAsJsonAsync("Inventory/", item);

    public async Task<HttpStatusCode> DeleteFromInventory(int position)
    {
        var response = await _http.DeleteAsync($"Inventory/{position}");
        
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> UpdateInventory(InventoryModel item)
    {
        var response = await _http.PutAsJsonAsync("Inventory/", item);
        
        return response.StatusCode;
    }
}
