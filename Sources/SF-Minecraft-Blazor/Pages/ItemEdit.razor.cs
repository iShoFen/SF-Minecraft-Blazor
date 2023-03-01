using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Model.Services;
using SF_Minecraft_Blazor.Entity;
using SF_Minecraft_Blazor.Extensions;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemEdit
{
    [Inject]
    public IStringLocalizer<ItemEdit> Localizer { get; set; }
    [Inject] public IDataItemListService DataItemListService { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; }

    [Parameter] public int Id { get; set; }

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        try
        {
            var item = await DataItemListService.GetById(Id);

            // Set the model with the item
            itemEntity = item.ToEntity();

            StateHasChanged();

            await SnackbarStack.PushAsync($"The item with {Id} has been successfully loaded", SnackbarColor.Info);
        }
        catch (Exception e)
        {
            await SnackbarStack.PushAsync($"Cannot get the item with id {Id} ", SnackbarColor.Danger);
        }
    }

    private async Task HandleValidSubmit()
    {
        await DataItemListService.Update(itemEntity.Id, itemEntity.ToModel());
        NavigationManager.NavigateTo("items");
        await SnackbarStack.PushAsync("Item updated successfully", SnackbarColor.Success);
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