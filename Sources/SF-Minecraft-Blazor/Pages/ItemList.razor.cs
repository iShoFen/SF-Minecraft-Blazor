using Blazored.Modal;
using Blazored.Modal.Services;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Model;
using Model.Services;
using SF_Minecraft_Blazor.Modals;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemList
{
    [Inject] public IDataItemListService DataItemListService { get; set; }

    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }

    [CascadingParameter] public IModalService Modal { get; set; }

    private DataGrid<Item>? itemGrid;

    private IEnumerable<Model.Item> Items { get; set; } = new List<Model.Item>();

    private int TotalItems { get; set; }

    private async Task OnReadData(DataGridReadDataEventArgs<Model.Item> e)
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
            catch (Exception exception)
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

        await DataItemListService.Delete(id);

        // Refresh the grid
        itemGrid?.Reload();
    }
}