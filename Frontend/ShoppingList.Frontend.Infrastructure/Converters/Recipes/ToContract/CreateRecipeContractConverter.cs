using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToContract;

public class CreateRecipeContractConverter : IToContractConverter<EditedRecipe, CreateRecipeContract>
{
    public CreateRecipeContract ToContract(EditedRecipe source)
    {
        return new CreateRecipeContract(
            source.Name,
            source.Ingredients.Select(i => new CreateIngredientContract(
                i.ItemCategoryId,
                i.QuantityTypeId,
                i.Quantity,
                i.DefaultItemId,
                i.DefaultItemTypeId)),
            source.PreparationSteps.Select(p => new CreatePreparationStepContract(
                p.Name,
                p.SortingIndex)),
            source.RecipeTagIds);
    }
}