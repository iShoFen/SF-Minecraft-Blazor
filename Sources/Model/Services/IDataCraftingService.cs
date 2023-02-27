namespace Model.Services;

/// <summary>
/// This interface is used to define the methods that will be used to interact with the crafting.
/// </summary>
public interface IDataCraftingService
{
    /// <summary>
    /// Get the recipes.
    /// </summary>
    /// <returns>The recipes.</returns>
    Task<List<Recipe>> GetRecipe();

    /// <summary>
    /// Reset the recipes.
    /// </summary>
    /// <returns>The async task.</returns>
    Task ResetRecipes();
}
