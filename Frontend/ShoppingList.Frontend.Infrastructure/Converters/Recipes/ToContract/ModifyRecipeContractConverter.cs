using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToContract;

public class ModifyRecipeContractConverter : IToContractConverter<Recipe, ModifyRecipeContract>
{
    public ModifyRecipeContract ToContract(Recipe source)
    {
        return new ModifyRecipeContract(
            source.Name,
            source.Ingredients.Select(i => new ModifyIngredientContract(
                i.Id,
                i.ItemCategoryId,
                i.QuantityType,
                i.Quantity,
                i.DefaultItemId,
                i.DefaultItemTypeId)),
            source.PreparationSteps.Select(p => new ModifyPreparationStepContract(
                p.Id,
                p.Name,
                p.SortingIndex)));
    }
}