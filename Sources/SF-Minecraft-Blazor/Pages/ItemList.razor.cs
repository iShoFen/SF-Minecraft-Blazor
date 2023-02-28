using System.Net;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Model.Item;
using Model.Services;
using SF_Minecraft_Blazor.Modals;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemList
{
    [Inject] public IDataItemListService DataItemListService { get; set; }

    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }

    [CascadingParameter] public IModalService Modal { get; set; }

    private DataGrid<Item>? itemGrid;

    private IEnumerable<Item> Items { get; set; } = new List<Item>();

    private int TotalItems { get; set; }

    private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
    {
        if (e.CancellationToken.IsCancellationRequested) return;

        if (!e.CancellationToken.IsCancellationRequested)
        {
            try
            {
                TotalItems = await DataItemListService.Count();
                Items = await DataItemListService.List(e.Page, e.PageSize);
                await SnackbarStack.PushAsync("Data loaded successfully", SnackbarColor.Info);
            }
            catch (Exception)
            {
                await SnackbarStack.PushAsync("Cannot load data from data source", SnackbarColor.Danger);
            }
        }
    }

    private async Task OnDelete(int id)
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(Item.Id), id);

        var modal = Modal.Show<DeleteItemConfirmation>("Delete Confirmation", parameters);
        var result = await modal.Result;

        if (result.Cancelled)
        {
            return;
        }

        var code = await DataItemListService.Delete(id);
        
        if (code == HttpStatusCode.OK)
        {
            await SnackbarStack.PushAsync("Item deleted successfully", SnackbarColor.Success);
        }
        else
        {
            await SnackbarStack.PushAsync("Cannot delete item", SnackbarColor.Danger);
        }
        

        // Refresh the grid
        itemGrid?.Reload();
    }
}