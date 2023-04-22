using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToContract;

public class ModifyRecipeContractConverter : IToContractConverter<EditedRecipe, ModifyRecipeContract>
{
    public ModifyRecipeContract ToContract(EditedRecipe source)
    {
        return new ModifyRecipeContract(
            source.Name,
            source.Ingredients.Select(i => new ModifyIngredientContract(
                i.Id == Guid.Empty ? null : i.Id,
                i.ItemCategoryId,
                i.QuantityTypeId,
                i.Quantity,
                i.DefaultItemId,
                i.DefaultItemTypeId,
                i.DefaultStoreId,
                i.AddToShoppingListByDefault)),
            source.PreparationSteps.Select(p => new ModifyPreparationStepContract(
                p.Id == Guid.Empty ? null : p.Id,
                p.Name,
                p.SortingIndex)),
            source.RecipeTagIds);
    }
}