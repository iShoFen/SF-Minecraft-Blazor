using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Model;
using Model.Services;

namespace SF_Minecraft_Blazor.Pages;

public partial class ItemList
{
    [Inject] public IDataItemListService DataItemListService { get; set; }

    private IEnumerable<Item> Items { get; set; } = new List<Item>();

    private int TotalItems { get; set; }

    private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
    {
        if (e.CancellationToken.IsCancellationRequested) return;

        if (!e.CancellationToken.IsCancellationRequested)
        {
            TotalItems = await DataItemListService.Count();
            Items = await DataItemListService.List(e.Page, e.PageSize);
        }
    }
}