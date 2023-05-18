using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class RecipeContractConverter : IToContractConverter<IRecipe, RecipeContract>
{
    public RecipeContract ToContract(IRecipe source)
    {
        var steps = source.PreparationSteps.Select(s => new PreparationStepContract(
            s.Id,
            s.Instruction.Value,
            s.SortingIndex));

        var ingredients = source.Ingredients.Select(i => new IngredientContract(
            i.Id,
            i.ItemCategoryId,
            i.QuantityType.ToInt(),
            i.Quantity.Value,
            i.ShoppingListProperties?.DefaultItemId,
            i.ShoppingListProperties?.DefaultItemTypeId,
            i.ShoppingListProperties?.DefaultStoreId,
            i.ShoppingListProperties?.AddToShoppingListByDefault));

        return new RecipeContract(
            source.Id,
            source.Name,
            source.NumberOfServings,
            ingredients,
            steps,
            source.Tags.Select(t => t.Value));
    }
}