using Model.Item;

namespace SF_Minecraft_Blazor.Components;

/// <summary>
/// The inventory action.
/// </summary>
public class InventoryAction
{
    /// <summary>
    /// The action.
    /// </summary>
    public string Action { get; set; } = "";
    
    /// <summary>
    /// The index of the item.
    /// </summary>
    public int Index { get; set; }
    
    /// <summary>
    /// The item.
    /// </summary>
    public Item? Item { get; set; }
}
