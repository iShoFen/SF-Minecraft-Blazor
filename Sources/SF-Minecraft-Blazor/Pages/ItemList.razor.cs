using System.Net;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Model.Item;
using Model.Services;
using SF_Minecraft_Blazor.Modals;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemList
{
    [Inject]
    public IStringLocalizer<ItemList> Localizer { get; set; }

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
                await SnackbarStack.PushAsync(Localizer["ItemsLoadedSuccessfully"], SnackbarColor.Info);
            }
            catch (Exception)
            {
                if (SnackbarStack != null)
                {
                    await SnackbarStack.PushAsync(Localizer["CannotLoadItemsFromDataSource"], SnackbarColor.Danger);
                }
            }
        }
    }

    private async Task OnDelete(int id)
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(Item.Id), id);

        var modal = Modal.Show<DeleteItemConfirmation>(Localizer["DeleteConfirmation"], parameters);
        var result = await modal.Result;

        if (result.Cancelled)
        {
            return;
        }

        var code = await DataItemListService.Delete(id);
        
        if (code == HttpStatusCode.OK)
        {
            await SnackbarStack.PushAsync(Localizer["ItemDeleteSuccessfully"], SnackbarColor.Success);
        }
        else
        {
            await SnackbarStack.PushAsync(Localizer["CannotDeleteItem"], SnackbarColor.Danger);
        }
        

        // Refresh the grid
        itemGrid?.Reload();
    }
}