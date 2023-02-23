using SF_Minecraft_Blazor.Data;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemList
{
    private IEnumerable<Item> _items = new List<Item>();

    private int _totalItem;

    private void OnReadData()
    {
        _items = new List<Item>
        {
            new()
            {
                Id = 1,
                DisplayName = "Item 1",
                Name = "item1",
                CreatedDate = DateTime.Now,
                EnchantCategories = new List<string>(),
                RepairWith = new List<string>(),
                MaxDurability = 4,
                StackSize = 45
            },
            new()
            {
                Id = 1,
                DisplayName = "Item 1",
                Name = "item1",
                CreatedDate = DateTime.Now,
                EnchantCategories = new List<string>(),
                RepairWith = new List<string>(),
                MaxDurability = 4,
                StackSize = 45
            },
            new()
            {
                Id = 1,
                DisplayName = "Item 1",
                Name = "item1",
                CreatedDate = DateTime.Now,
                EnchantCategories = new List<string>(),
                RepairWith = new List<string>(),
                MaxDurability = 4,
                StackSize = 45
            }
        };
        _totalItem = _items.Count();
    }
}