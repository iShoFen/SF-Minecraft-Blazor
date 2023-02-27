using Blazorise.DataGrid;
using Model;

namespace SF_Minecraft_Blazor.Pages;

public partial class Inventory
{
    private List<Item> Items { get; set; } = new()
    {
        new Item
        {
            Id = 1,
            DisplayName = "Diamond",
            Name = "item1",
            CreatedDate = DateTime.Now,
            EnchantCategories = new List<string>(),
            RepairWith = new List<string>(),
            MaxDurability = 4,
            StackSize = 45
        },
        new Item
        {
            Id = 1,
            DisplayName = "Dirt",
            Name = "item1",
            CreatedDate = DateTime.Now,
            EnchantCategories = new List<string>(),
            RepairWith = new List<string>(),
            MaxDurability = 4,
            StackSize = 45
        },
        new Item
        {
            Id = 1,
            DisplayName = "Stone",
            Name = "item1",
            CreatedDate = DateTime.Now,
            EnchantCategories = new List<string>(),
            RepairWith = new List<string>(),
            MaxDurability = 4,
            StackSize = 45
        }
    };

    private int totalItems { get; set; }

    private string SearchValue { get; set; } = "";

    private DataGrid<Item> _dataGrid = null!;

    public Inventory()
    {
        totalItems = Items.Count;
    }

    private Task OnReadData(DataGridReadDataEventArgs<Item> arg)
    {
        return Task.CompletedTask;
    }

    private Task OnSearchRequested(string search)
    {
        SearchValue = search;
        return _dataGrid.Reload();
    }

    private bool OnCustomFilter(Item item)
    {
        if (string.IsNullOrEmpty(SearchValue))
            return true;
        return item.DisplayName.Contains(SearchValue, StringComparison.OrdinalIgnoreCase)
               || item.Name.Contains(SearchValue, StringComparison.OrdinalIgnoreCase);
    }
}