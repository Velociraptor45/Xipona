using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToContract;

public class CreateRecipeContractConverter : IToContractConverter<EditedRecipe, CreateRecipeContract>
{
    public CreateRecipeContract ToContract(EditedRecipe source)
    {
        return new CreateRecipeContract(
            source.Name,
            source.NumberOfServings,
            source.Ingredients.Select(i => new CreateIngredientContract(
                i.ItemCategoryId,
                i.QuantityTypeId,
                i.Quantity,
                i.DefaultItemId,
                i.DefaultItemTypeId,
                i.DefaultStoreId,
                i.AddToShoppingListByDefault)),
            source.PreparationSteps.Select(p => new CreatePreparationStepContract(
                p.Name,
                p.SortingIndex)),
            source.RecipeTagIds,
            source.SideDish?.Id);
    }
}