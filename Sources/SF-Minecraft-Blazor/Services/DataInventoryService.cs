using System.Net;
using Model.Inventory;
using Model.Services;

namespace SF_Minecraft_Blazor.Services;

public class DataInventoryService: IDataInventoryService
{
    private readonly string _baseUrl;
         
    private readonly HttpClient _http;
     
    public DataInventoryService(HttpClient http, string baseUrl)
    {
        _baseUrl = baseUrl;
        _http = http;
    }
    
    public async Task<List<InventoryModel>> GetInventory() 
        => await _http.GetFromJsonAsync<List<InventoryModel>>(_baseUrl + "Inventory/") ?? new List<InventoryModel>();
    
    public async Task AddToInventory(InventoryModel item) 
        => await _http.PostAsJsonAsync(_baseUrl + "Inventory/", item);

    public async Task<HttpStatusCode> DeleteFromInventory(int position)
    {
        var response = await _http.DeleteAsync(_baseUrl + $"Inventory/{position}");
        
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> UpdateInventory(InventoryModel item)
    {
        var response = await _http.PutAsJsonAsync(_baseUrl + "Inventory/", item);
        
        return response.StatusCode;
    }
}
