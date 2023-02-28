using System.Net;
using Model.Item;
using Model.Services;

namespace SF_Minecraft_Blazor.Services;

public class DataItemListService: IDataItemListService
{ 
    private readonly HttpClient _http;
     
    public DataItemListService(HttpClient http)
    {
         _http = http;
    }
    
    public async Task Add(Item item) 
    // add Crafting/ to the end of the URL
        => await _http.PostAsJsonAsync("Crafting/", item);

    public async Task<HttpStatusCode> Delete(int id)
    {
        var response = await _http.DeleteAsync($"Crafting/{id}");

        return response.StatusCode;
    }

    public async Task<HttpStatusCode> Update(int id, Item item)
    {
        var response = await _http.PutAsJsonAsync($"Crafting/{id}", item);
        
        return response.StatusCode;
    }

    public async Task<IEnumerable<Item>> All()
        => await _http.GetFromJsonAsync<List<Item>>("Crafting/all") ?? new List<Item>();

    public async Task<IEnumerable<Item>> List(int page, int pageSize)
        => await _http.GetFromJsonAsync<List<Item>>($"Crafting/?currentPage={page}&pageSize={pageSize}")
           ?? new List<Item>();

    public async Task<int> Count()
        => await _http.GetFromJsonAsync<int>("Crafting/count");

    public async Task<Item> GetById(int id)
        => (await _http.GetFromJsonAsync<Item>($"Crafting/{id}"))!;

    public async Task<Item> GetByName(string name)
        => (await _http.GetFromJsonAsync<Item>($"Crafting/by-name/{name}"))!;

    public async Task ResetItems()
        => await _http.PostAsync("Crafting/reset-items", null);
}
