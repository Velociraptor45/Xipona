using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToContract;

public class CreateRecipeContractConverter : IToContractConverter<Recipe, CreateRecipeContract>
{
    public CreateRecipeContract ToContract(Recipe source)
    {
        return new CreateRecipeContract(
            source.Name,
            source.Ingredients.Select(i => new CreateIngredientContract(
                i.ItemCategoryId,
                i.QuantityType,
                i.Quantity,
                i.DefaultItemId,
                i.DefaultItemTypeId)),
            source.PreparationSteps.Select(p => new CreatePreparationStepContract(
                p.Instruction,
                p.SortingIndex)));
    }
}