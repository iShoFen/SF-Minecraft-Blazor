using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Model.Services;
using SF_Minecraft_Blazor.Entity;
using SF_Minecraft_Blazor.Extensions;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemAdd
{
    [Inject] public IDataItemListService DataService { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }

    /// <summary>
    /// The default enchant categories.
    /// </summary>
    private List<string> enchantCategories = new()
        { "armor", "armor_head", "armor_chest", "weapon", "digger", "breakable", "vanishable" };

    /// <summary>
    /// The default repair with.
    /// </summary>
    private List<string> repairWith = new()
    {
        "oak_planks", "spruce_planks", "birch_planks", "jungle_planks", "acacia_planks", "dark_oak_planks",
        "crimson_planks", "warped_planks"
    };

    /// <summary>
    /// The current item model
    /// </summary>
    private ItemEntity itemEntity = new()
    {
        EnchantCategories = new List<string>(),
        RepairWith = new List<string>()
    };

    private async Task HandleValidSubmit()
    {
        await DataService.Add(itemEntity.ToModel());
        NavigationManager.NavigateTo("items");
        await SnackbarStack.PushAsync("Item added successfully", SnackbarColor.Success);
    }

    private async Task LoadImage(InputFileChangeEventArgs e)
    {
        // Set the content of the image to the model
        using var memoryStream = new MemoryStream();
        await e.File.OpenReadStream().CopyToAsync(memoryStream);
        itemEntity.ImageBase64 = Convert.ToBase64String(memoryStream.ToArray());
    }

    private void OnEnchantCategoriesChange(string item, object? checkedValue)
    {
        if ((bool)(checkedValue ?? false))
        {
            if (!itemEntity.EnchantCategories.Contains(item))
            {
                itemEntity.EnchantCategories.Add(item);
            }

            return;
        }

        if (itemEntity.EnchantCategories.Contains(item))
        {
            itemEntity.EnchantCategories.Remove(item);
        }
    }

    private void OnRepairWithChange(string item, object? checkedValue)
    {
        if ((bool)(checkedValue ?? false))
        {
            if (!itemEntity.RepairWith.Contains(item))
            {
                itemEntity.RepairWith.Add(item);
            }

            return;
        }

        if (itemEntity.RepairWith.Contains(item))
        {
            itemEntity.RepairWith.Remove(item);
        }
    }
}