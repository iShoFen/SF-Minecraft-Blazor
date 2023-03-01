using System.Net;
using Model.Item;
using Model.Services;

namespace SF_Minecraft_Blazor.Services;

public class DataItemListService: IDataItemListService
{ 
    private readonly HttpClient _http;
    
    private readonly ILogger<DataItemListService> _logger;

    public DataItemListService(HttpClient http, ILogger<DataItemListService> logger)
    {
        _http = http;
        _logger = logger;
        
        _logger.LogInformation("DataItemListService created");
    }

    public async Task Add(Item item)
    {
        var response = await _http.PostAsJsonAsync("Crafting/", item);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("Add failed with {StatusCode}", response.StatusCode);
        }
        
        _logger.LogInformation("Add succeeded with {StatusCode}", response.StatusCode);
    }

    public async Task<HttpStatusCode> Delete(int id)
    {
        var response = await _http.DeleteAsync($"Crafting/{id}");
        if (response.StatusCode != HttpStatusCode.OK) _logger.LogWarning("Delete failed with {StatusCode}", response.StatusCode);
        else _logger.LogInformation("Delete succeeded with {StatusCode}", response.StatusCode);

        return response.StatusCode;
    }

    public async Task<HttpStatusCode> Update(int id, Item item)
    {
        var response = await _http.PutAsJsonAsync($"Crafting/{id}", item);
        if (response.StatusCode != HttpStatusCode.OK) _logger.LogWarning("Update failed with {StatusCode}", response.StatusCode);
        else _logger.LogInformation("Update succeeded with {StatusCode}", response.StatusCode);
        
        return response.StatusCode;
    }

    public async Task<IEnumerable<Item>> All()
    {
        var response = await _http.GetFromJsonAsync<List<Item>>("Crafting/all");
        if (response == null)
        {
            _logger.LogWarning("All returned null");
            return new List<Item>();
        }
        _logger.LogInformation("{ItemCount} items retrieved", response.Count);
        
        return response;
    }

    public async Task<IEnumerable<Item>> List(int page, int pageSize)
    {
        var response = await _http.GetFromJsonAsync<List<Item>>($"Crafting/?currentPage={page}&pageSize={pageSize}");
        if (response == null)
        {
            _logger.LogWarning("List returned null");
            return new List<Item>();
        }
        _logger.LogInformation("{ItemCount} items retrieved", response.Count);
        
        return response;
    }

    public async Task<int> Count()
    {
        var response =  await _http.GetFromJsonAsync<int>("Crafting/count");
        if (response == 0)
        {
            _logger.LogWarning("Count returned 0");
        }
        _logger.LogInformation("{ItemCount} items retrieved", response);
        
        return response;
    }

    public async Task<Item> GetById(int id)
    {
        var response = await _http.GetFromJsonAsync<Item>($"Crafting/{id}");
        if (response == null)
        {
            _logger.LogWarning("Item {ItemId} not found", id);
            throw new ArgumentException($"Item with id {id} not found");
        }
        _logger.LogInformation("Item {ItemId} retrieved", id);
        
        return response;
    }

    public async Task<Item> GetByName(string name)
    {
        var response = await _http.GetFromJsonAsync<Item>($"Crafting/by-name/{name}");
        if (response == null)
        {
            _logger.LogWarning("Item {ItemName} not found", name);
            throw new ArgumentException($"Item with name {name} not found");
        }
        _logger.LogInformation("Item {ItemName} retrieved", name);
        
        return response;
    }

    public async Task ResetItems()
    { 
        var response = await _http.PostAsync("Crafting/reset-items", null);
        if (response.StatusCode != HttpStatusCode.OK) _logger.LogWarning("ResetItems failed with {StatusCode}", response.StatusCode);
        else _logger.LogInformation("ResetItems succeeded with {StatusCode}", response.StatusCode);
    }
}
