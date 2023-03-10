using System.ComponentModel.DataAnnotations;

namespace SF_Minecraft_Blazor.Entity;

public class InventoryEntity
{
    [Required(ErrorMessage = "The item id is required.")]
    public int ItemId { get; set; }
    
    public string ImageBase64 { get; set; }
    
    [Required]
    [Range(1,64, ErrorMessage = "The number of item must be between 1 and 64.")]
    public int NumberItem { get; set; }
    
    [Required(ErrorMessage = "The position is required.")]
    public int Position { get; set; }
}
