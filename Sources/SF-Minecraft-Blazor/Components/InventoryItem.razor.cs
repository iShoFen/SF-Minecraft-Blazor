using Microsoft.AspNetCore.Components;
using Model.Item;
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

    /// <summary>
    /// If the item can be dropped.
    /// </summary>
    [Parameter]
    public bool NoDrop { get; set; }
    
    /// <summary>
    /// The parent component.
    /// </summary>
    [CascadingParameter]
    public MyInventory Parent { get; set; } = null!;

    /// <summary>
    /// When entering to drag.
    /// </summary>
    internal void OnDragEnter()
    {
        if (NoDrop) return;
        
        Parent.Actions.Add(new InventoryAction { Action = "Drag Enter", Item = Item, Index = Index });
    }

    /// <summary>
    /// When leaving the drag.
    /// </summary>
    internal void OnDragLeave()
    {
        if (NoDrop) return;
        
        Parent.Actions.Add(new InventoryAction { Action = "Drag Leave", Item = Item, Index = Index });
    }

    /// <summary>
    /// When dropping the item.
    /// </summary>
    internal void OnDrop()
    {
        if (NoDrop)
        {
            return;
        }

        Item = Parent.CurrentDragItem;
        Parent.Actions.Add(new InventoryAction { Action = "Drop", Item = Item, Index = Index });
    }

    /// <summary>
    /// When starting to drag.
    /// </summary>
    private void OnDragStart()
    {
        Parent.CurrentDragItem = Item;
        Parent.Actions.Add(new InventoryAction { Action = "Drag Start", Item = Item, Index = Index });
    }
}