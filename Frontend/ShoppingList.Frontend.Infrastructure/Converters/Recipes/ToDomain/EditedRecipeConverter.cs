using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Recipes.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class EditedRecipeConverter : IToDomainConverter<RecipeContract, EditedRecipe>
{
    private readonly IToDomainConverter<IngredientContract, EditedIngredient> _ingredientConverter;
    private readonly IToDomainConverter<PreparationStepContract, EditedPreparationStep> _preparationStepConverter;

    public EditedRecipeConverter(
        IToDomainConverter<IngredientContract, EditedIngredient> ingredientConverter,
        IToDomainConverter<PreparationStepContract, EditedPreparationStep> preparationStepConverter)
    {
        _ingredientConverter = ingredientConverter;
        _preparationStepConverter = preparationStepConverter;
    }

    public EditedRecipe ToDomain(RecipeContract source)
    {
        return new EditedRecipe(
            source.Id,
            source.Name,
            _ingredientConverter.ToDomain(source.Ingredients).ToList(),
            _preparationStepConverter.ToDomain(source.PreparationSteps).ToList());
    }
}