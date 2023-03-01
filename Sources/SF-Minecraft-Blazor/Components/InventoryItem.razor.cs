using System.Net;
using System.Reflection.Metadata;
using Blazorise;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Model.Inventory;
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
    /// The parent component.
    /// </summary>
    [CascadingParameter]
    public Inventory Inventory { get; set; }
    
    [CascadingParameter]
    public MyInventory MyInventory { get; set; }

    [CascadingParameter] public List<InventoryEntity> Items { get; set; }

    [Inject] public IDataItemListService DataItemListService { get; set; }

    [Inject] public IDataInventoryService DataInventoryService { get; set; }

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
        if (Inventory.CurrentDragItem != null)
        {
            MyInventory.Actions.Add(new InventoryAction { Action = "Drag Enter", Item = Item, Index = Index });
            Inventory.CurrentDragItem.Position = Index;
        }
    }

    /// <summary>
    /// When leaving the drag.
    /// </summary>
    internal void OnDragLeave()
    {
        if (Inventory.CurrentDragItem != null)
        {
            MyInventory.Actions.Add(new InventoryAction { Action = "Drag Leave", Item = Item, Index = Index });
            Inventory.CurrentDragItem.Position = -1;
        }
    }

    /// <summary>
    /// When dropping the item.
    /// </summary>
    internal async Task OnDrop()
    {
        var currentDragItem = Inventory.CurrentDragItem;

        if (currentDragItem != null)
        {
            MyInventory.Actions.Add(new InventoryAction { Action = "Drop", Item = Item, Index = Index });
            Inventory.CurrentDragItem!.Position = Index;
            if (currentDragItem.StartPosition == Index) return;

            if (Item == null)
            {
                Item = currentDragItem.Item;
                Count = currentDragItem.Count;
                try
                {
                    await DataInventoryService.AddToInventory(new InventoryModel
                    {
                        Position = Index,
                        ItemId = Item.Id,
                        NumberItem = Count
                    });
                    await SnackbarStack.PushAsync($"The item {Item.DisplayName} was added successfully",
                        SnackbarColor.Success);
                }
                catch (Exception e)
                {
                    await SnackbarStack.PushAsync("Error while updating inventory",
                        SnackbarColor.Danger);
                }
            }
            else
            {
                if (currentDragItem.Item.Id == Item?.Id)
                {
                    Count += currentDragItem.Count;
                    try
                    {
                        var result = await DataInventoryService.UpdateInventory(new InventoryModel
                        {
                            Position = Index,
                            ItemId = Item.Id,
                            NumberItem = Count
                        });
                        if (result == HttpStatusCode.OK)
                        {
                            await SnackbarStack.PushAsync($"The item {Item.DisplayName} was updated successfully",
                                SnackbarColor.Success);
                        }
                        else
                        {
                            await SnackbarStack.PushAsync("Error while updating inventory",
                                SnackbarColor.Danger);
                        }
                    }
                    catch (Exception e)
                    {
                        await SnackbarStack.PushAsync("Error while updating inventory",
                            SnackbarColor.Danger);
                    }
                }
                else
                {
                    currentDragItem.DeleteStartItem = false;
                    await SnackbarStack.PushAsync("Cannot override item because it is not the same",
                        SnackbarColor.Danger);
                }
            }

            // try
            // {
            //     await DataInventoryService.DeleteFromInventory(currentDragItem.StartPosition);
            // }
            // catch (Exception e)
            // {
            //     await SnackbarStack.PushAsync(
            //         $"Error while transfer item from {currentDragItem.StartPosition} to {Index}", SnackbarColor.Danger);
            // }
        }
    }

    /// <summary>
    /// When starting to drag.
    /// </summary>
    private void OnDragStart()
    {
        if (Item != null)
        {
            MyInventory.Actions.Add(new InventoryAction { Action = "Drag Start", Item = Item, Index = Index });
            Inventory.CurrentDragItem = new InventoryTransferItem
            {
                Item = Item,
                Count = Count,
                Position = Index,
                StartPosition = Index,
                DeleteStartItem = true
            };
        }
    }

    private async Task OnDragEnd()
    {
        var currentDragItem = Inventory.CurrentDragItem;

        if (currentDragItem == null
            || (currentDragItem.StartPosition == currentDragItem.Position
                && Index == currentDragItem.StartPosition)) return;

        if (currentDragItem.DeleteStartItem)
        {
            MyInventory.Actions.Add(new InventoryAction { Action = "Drag End", Item = Item, Index = Index });
            try
            {
                // case when OnDrop was not triggered
                await DataInventoryService.DeleteFromInventory(currentDragItem.StartPosition);

                await SnackbarStack.PushAsync($"The item {Item!.DisplayName} was deleted successfully",
                    SnackbarColor.Success);

                Item = null;
            }
            catch (Exception e)
            {
                await SnackbarStack.PushAsync("Error while deleting inventory",
                    SnackbarColor.Danger);
            }
        }

        Inventory.CurrentDragItem = null;
    }
}