using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IRecipe
{
    RecipeId Id { get; }
    RecipeName Name { get; }
    IReadOnlyCollection<IIngredient> Ingredients { get; }
    IReadOnlyCollection<IPreparationStep> PreparationSteps { get; }
    Task ModifyAsync(RecipeModification modification, IValidator validator);
    void RemoveDefaultItem(ItemId defaultItemId);
}