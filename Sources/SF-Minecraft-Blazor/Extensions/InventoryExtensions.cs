using Model.Inventory;
using SF_Minecraft_Blazor.Entity;

namespace SF_Minecraft_Blazor.Extensions;

public static class InventoryExtensions
{
    public static InventoryModel  ToModel(this InventoryEntity entity)
        => new()
        {
            ItemId = entity.ItemId,
            NumberItem = entity.NumberItem,
            Position = entity.Position
        };
    
    public static InventoryEntity ToEntity(this InventoryModel model)
        => new()
        {
            ItemId = model.ItemId,
            NumberItem = model.NumberItem,
            Position = model.Position
        };
}
