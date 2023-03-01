using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Model.Item;
using Model.Services;
using SF_Minecraft_Blazor.Entity;

namespace SF_Minecraft_Blazor.Pages;

public partial class Inventory
{
    [Inject]
    public IStringLocalizer<Inventory> Localizer { get; set; }
    
    /// <summary>
    /// The current drag item.
    /// </summary>
    public InventoryTransferItem? CurrentDragItem { get; set; }

    /// <summary>
    /// Injected service for accessing the inventory data.
    /// </summary>
    [Inject] public IDataInventoryService DataInventoryService { get; set; }
    
    /// <summary>
    /// Injected service for accessing the item data.
    /// </summary>
    [Inject] public IDataItemListService DataItemListService { get; set; }
    
    /// <summary>
    /// Injected logger.
    /// </summary>
    [Inject] public ILogger<Inventory> Logger { get; set; }

    /// <summary>
    /// The snackbar stack.
    /// </summary>
    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }
    
    /// <summary>
    /// The data grid for the items.
    /// </summary>
    private DataGrid<Item> _itemGrid = null!;
    
    /// <summary>
    /// The items to display.
    /// </summary>
    private List<Item> DisplayItems { get; set; } = new();
    
    /// <summary>
    /// All the items.
    /// </summary>
    private List<Item> _items = new();

    /// <summary>
    /// The total number of items.
    /// </summary>
    private int TotalItems { get; set; } = -1;
    
    /// <summary>
    /// The search value query.
    /// </summary>
    private string SearchValue { get; set; } = "";
    
    private async Task OnReadData(DataGridReadDataEventArgs<Item> arg)
    {
        if (arg.CancellationToken.IsCancellationRequested) return;
        
        try
        {
            if (TotalItems == -1)
            {
                _items = (await DataItemListService.All()).ToList();
                TotalItems = _items.Count;
                DisplayItems = _items.Skip((arg.Page - 1) * arg.PageSize).Take(arg.PageSize).ToList();
                await SnackbarStack.PushAsync(Localizer["ItemsLoadedSuccessfully"], SnackbarColor.Info);
            }
            else
            {
                var filteredItems = _items.Where(OnCustomFilter).ToList();
                TotalItems = filteredItems.Count;
                DisplayItems = filteredItems.Skip((arg.Page - 1) * arg.PageSize).Take(arg.PageSize).ToList();
            }
        }
        catch (Exception)
        {
            await SnackbarStack.PushAsync(Localizer["CannotLoadDataFromDataSource"], SnackbarColor.Danger);
        }
    }

    private Task OnSearchRequested(string search)
    {
        SearchValue = search;
        Logger.LogInformation("Search requested: {SearchValue}", SearchValue);
        return _itemGrid.Reload();
    }

    private bool OnCustomFilter(Item item) 
        => string.IsNullOrEmpty(SearchValue) || item.DisplayName.StartsWith(SearchValue, StringComparison.OrdinalIgnoreCase);

    private void OnDragStart(Item item)
    {
        CurrentDragItem = new InventoryTransferItem
        {
            Item = item,
            Count = 1,
            Position = -1
        };
    }
}