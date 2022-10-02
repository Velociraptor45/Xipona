using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class RecipeConverter : IToDomainConverter<RecipeContract, Recipe>
{
    private readonly IToDomainConverter<IngredientContract, Ingredient> _ingredientConverter;
    private readonly IToDomainConverter<PreparationStepContract, PreparationStep> _preparationStepConverter;

    public RecipeConverter(
        IToDomainConverter<IngredientContract, Ingredient> ingredientConverter,
        IToDomainConverter<PreparationStepContract, PreparationStep> preparationStepConverter)
    {
        _ingredientConverter = ingredientConverter;
        _preparationStepConverter = preparationStepConverter;
    }

    public Recipe ToDomain(RecipeContract source)
    {
        return new Recipe(
            source.Id,
            source.Name,
            _ingredientConverter.ToDomain(source.Ingredients),
            _preparationStepConverter.ToDomain(source.PreparationSteps));
    }
}