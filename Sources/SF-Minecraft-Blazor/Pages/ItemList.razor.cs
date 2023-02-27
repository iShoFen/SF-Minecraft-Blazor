using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Model.Services;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemList
{
    [Inject] public IDataItemListService DataItemListService { get; set; }
    
    [CascadingParameter] public SnackbarStack SnackbarStack { get; set; }

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
}