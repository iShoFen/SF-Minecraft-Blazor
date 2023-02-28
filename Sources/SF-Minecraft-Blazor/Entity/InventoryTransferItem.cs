using Model.Item;

namespace SF_Minecraft_Blazor.Entity;

public class InventoryTransferItem
{
    public Item Item { get; set; }
    
    public int Position { get; set; }
    
    public int Count { get; set; }
}