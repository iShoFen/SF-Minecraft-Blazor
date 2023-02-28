using System.ComponentModel.DataAnnotations;

namespace SF_Minecraft_Blazor.Entity;

public class ItemEntity
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "The display name must not exceed 50 characters.")]
    public string DisplayName { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "The name must not exceed 50 characters.")]
    [RegularExpression(@"^[a-z''-'\s]{1,50}$", ErrorMessage = "Only lowercase characters are accepted.")]
    public string Name { get; set; }
    
    [Required] [Range(1,64)] public int StackSize { get; set; }
    
    [Required] [Range(1,125)] public int MaxDurability { get; set; }
    
    public IList<string> EnchantCategories { get; set; }
    
    public IList<string> RepairWith { get; set; }
    
    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions.")]
    public bool AcceptTerms { get; set; }
    
    [Required(ErrorMessage = "The image is required.")]
    public string ImageBase64 { get; set; }
}
