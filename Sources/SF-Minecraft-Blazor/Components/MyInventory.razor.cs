using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model.Item;

namespace SF_Minecraft_Blazor.Components;

public partial class MyInventory
{
    /// <summary>
    /// The actions.
    /// </summary>
    public ObservableCollection<InventoryAction> Actions { get; set; } = new();

    /// <summary>
    /// The current drag item.
    /// </summary>
    public Item? CurrentDragItem { get; set; }
    
    /// <summary>
    /// All the items.
    /// </summary>
    [Parameter]
    public List<Item> Items { get; set; }
    
    /// <summary>
    /// Gets or sets the java script runtime.
    /// </summary>
    [Inject]
    internal IJSRuntime JavaScriptRuntime { get; set; }

    public MyInventory()
    {
        Actions.CollectionChanged += OnActionsCollectionChanged;
    }
    
    private void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        JavaScriptRuntime.InvokeVoidAsync("Crafting.AddActions", e.NewItems);
    }
}
