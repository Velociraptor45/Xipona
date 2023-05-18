using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IRecipe
{
    RecipeId Id { get; }
    RecipeName Name { get; }
    NumberOfServings NumberOfServings { get; }
    IReadOnlyCollection<IIngredient> Ingredients { get; }
    IReadOnlyCollection<IPreparationStep> PreparationSteps { get; }
    IReadOnlyCollection<RecipeTagId> Tags { get; }

    Task ModifyAsync(RecipeModification modification, IValidator validator);

    void RemoveDefaultItem(ItemId defaultItemId);

    void ModifyIngredientsAfterItemUpdate(ItemId oldItemId, IItem newItem);
}