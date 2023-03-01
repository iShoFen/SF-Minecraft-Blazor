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
    /// The actions.
    /// </summary>
    public ObservableCollection<InventoryAction> Actions { get; set; } = new();

    /// <summary>
    /// All the items.
    /// </summary>
    public List<InventoryEntity> Items { get; set; }

    [Inject] public IDataInventoryService DataInventoryService { get; set; }

    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }

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
        JavaScriptRuntime.InvokeVoidAsync("MyInventory.AddActions", e.NewItems);
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Items = (await DataInventoryService.GetInventory()).Select(item => item.ToEntity()).ToList();
            await SnackbarStack.PushAsync("Inventory loaded", SnackbarColor.Info);
        }
        catch (Exception)
        {
            if (SnackbarStack != null)
            {
                await SnackbarStack.PushAsync("Cannot load inventory from data source", SnackbarColor.Danger);
            }
        }
    }
}