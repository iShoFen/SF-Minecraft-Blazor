using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model.Inventory;
using Model.Item;
using Model.Services;
using SF_Minecraft_Blazor.Entity;
using SF_Minecraft_Blazor.Extensions;

namespace SF_Minecraft_Blazor.Components;

public partial class MyInventory
{
    /// <summary>
    /// All the items.
    /// </summary>
    public List<InventoryEntity> Items { get; set; }

    [Inject] public IDataInventoryService DataInventoryService { get; set; }

    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        await base.OnAfterRenderAsync(firstRender);

        try
        {
            Items = (await DataInventoryService.GetInventory()).Select(item => item.ToEntity()).ToList();
        }
        catch (Exception)
        {
            await SnackbarStack.PushAsync("Cannot load inventory from data source", SnackbarColor.Danger);
        }
    }
}