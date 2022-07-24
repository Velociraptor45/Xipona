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
            s.Id.Value,
            s.Instruction.Value,
            s.SortingIndex));

        var ingredients = source.Ingredients.Select(i => new IngredientContract(
            i.Id.Value,
            i.ItemCategoryId.Value,
            i.QuantityType.ToInt(),
            i.Quantity.Value));

        return new RecipeContract(
            source.Id.Value,
            source.Name.Value,
            ingredients,
            steps);
    }
}