using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

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
        var preparationSteps = _preparationStepConverter.ToDomain(source.PreparationSteps);

        return new EditedRecipe(
            source.Id,
            source.Name,
            source.NumberOfServings,
            _ingredientConverter.ToDomain(source.Ingredients).ToList(),
            new SortedSet<EditedPreparationStep>(preparationSteps, new SortingIndexComparer()),
            source.RecipeTagIds.ToList());
    }
}