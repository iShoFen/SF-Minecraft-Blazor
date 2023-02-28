using System.Reflection.Metadata;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Model.Item;
using Model.Services;
using SF_Minecraft_Blazor.Entity;
using SF_Minecraft_Blazor.Pages;

namespace SF_Minecraft_Blazor.Components;

public partial class InventoryItem
{
    /// <summary>
    /// The index of the item.
    /// </summary>
    [Parameter]
    public int Index { get; set; }

    /// <summary>
    /// The item.
    /// </summary>
    [Parameter]
    public Item? Item { get; set; }
    
    public int Count { get; set; }

    /// <summary>
    /// If the item can be dropped.
    /// </summary>
    [Parameter]
    public bool NoDrop { get; set; }

    /// <summary>
    /// The parent component.
    /// </summary>
    [CascadingParameter]
    public Inventory Inventory { get; set; }
    
    [CascadingParameter]
    public List<InventoryEntity> Items { get; set; }

    [Inject] public IDataItemListService DataItemListService { get; set; }

    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        await base.OnAfterRenderAsync(firstRender);

        if (Items != null)
        {
            var inventoryEntity = Items.Find(item => item.Position == Index);
            if (inventoryEntity != null)
            {
                try
                {
                    Item = await DataItemListService.GetById(inventoryEntity.ItemId);
                    Count = inventoryEntity.NumberItem;
                    await SnackbarStack.PushAsync($"Item {Item.DisplayName} loaded", SnackbarColor.Success);
                }
                catch (Exception e)
                {
                    await SnackbarStack.PushAsync($"Cannot load item with id {inventoryEntity.ItemId} from data source",
                        SnackbarColor.Danger);
                }
            }
        }
    }

    /// <summary>
    /// When entering to drag.
    /// </summary>
    internal void OnDragEnter()
    {
        if (NoDrop) return;
    }

    /// <summary>
    /// When leaving the drag.
    /// </summary>
    internal void OnDragLeave()
    {
        if (NoDrop) return;
    }

    /// <summary>
    /// When dropping the item.
    /// </summary>
    internal async Task OnDrop()
    {
        if (NoDrop)
        {
            return;
        }

        var currentDragItem = Inventory.CurrentDragItem;

        if (currentDragItem != null)
        {
            if (Item == null)
            {
                Item = currentDragItem.Item;
                Count = currentDragItem.Count;
            }
            else
            {
                if (currentDragItem.Item.Id == Item?.Id)
                {
                    Count += currentDragItem.Count;
                }
                else
                {
                    await SnackbarStack.PushAsync("Cannot override item because it is not the same", SnackbarColor.Danger);
                }  
            }
        }
    }

    /// <summary>
    /// When starting to drag.
    /// </summary>
    private void OnDragStart()
    {
        Inventory.CurrentDragItem = new InventoryTransferItem
        {
            Item = Item!,
            Count = Count,
            Position = Index
        };
    }
}