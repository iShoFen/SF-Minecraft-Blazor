using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Model.Item;
using Model.Services;

namespace SF_Minecraft_Blazor.Modals;

public partial class DeleteItemConfirmation
{
    [CascadingParameter]
    public BlazoredModalInstance ModalInstance { get; set; }

    [Inject]
    public IDataItemListService DataService { get; set; }

    [Parameter]
    public int Id { get; set; }

    private Item item = new();

    protected override async Task OnInitializedAsync()
    {
        // Get the item
        item = await DataService.GetById(Id);
    }

    void ConfirmDelete()
    {
        ModalInstance.CloseAsync(ModalResult.Ok(true));
    }

    void Cancel()
    {
        ModalInstance.CancelAsync();
    }
}