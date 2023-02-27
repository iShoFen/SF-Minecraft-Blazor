using Model;
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
    
    public async Task AddToInventory(InventoryModel item) 
        => await _http.PostAsJsonAsync(_baseUrl + "Inventory/", item);

    public async Task DeleteFromInventory(InventoryModel item) 
        => await _http.DeleteAsync(_baseUrl + "Inventory/");

    public async Task<List<InventoryModel>> GetInventory() 
        => await _http.GetFromJsonAsync<List<InventoryModel>>(_baseUrl + "Inventory/") ?? new List<InventoryModel>();

    public async Task UpdateInventory(InventoryModel item) 
        => await _http.PutAsJsonAsync(_baseUrl + "Inventory/", item);
}
