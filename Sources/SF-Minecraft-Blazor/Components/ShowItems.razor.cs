using Microsoft.AspNetCore.Components;

namespace SF_Minecraft_Blazor.Components;

public partial class ShowItems<TItem>
{
    [Parameter]
    public List<TItem> Items { get; set; }

    [Parameter]
    public RenderFragment<TItem> ShowTemplate { get; set; }
}