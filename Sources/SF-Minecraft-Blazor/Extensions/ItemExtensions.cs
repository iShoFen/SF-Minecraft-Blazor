using Model;
using SF_Minecraft_Blazor.Model;

namespace SF_Minecraft_Blazor.Extensions;

public static class ItemExtensions
{
    public static Item ToModel(this ItemEntity entity)
        => new()
        {
            Id = entity.Id,
            DisplayName = entity.DisplayName,
            Name = entity.Name,
            StackSize = entity.StackSize,
            MaxDurability = entity.MaxDurability,
            EnchantCategories = entity.EnchantCategories.ToList(),
            RepairWith = entity.RepairWith.ToList(),
            ImageBase64 = Convert.ToBase64String(entity.Image)
        };
    
    public static ItemEntity ToEntity(this Item model) 
        => new() 
        {
            Id = model.Id,
            DisplayName = model.DisplayName,
            Name = model.Name,
            StackSize = model.StackSize,
            MaxDurability = model.MaxDurability,
            EnchantCategories = model.EnchantCategories,
            RepairWith = model.RepairWith,
            Image = Convert.FromBase64String(model.ImageBase64)
        };
}
