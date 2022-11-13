using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;

public class RecipeConverter : IToContractConverter<IRecipe, Entities.Recipe>
{
    private readonly IToContractConverter<(RecipeId, IIngredient), Entities.Ingredient> _ingredientConverter;
    private readonly IToContractConverter<(RecipeId, IPreparationStep), Entities.PreparationStep> _preparationStepConverter;

    public RecipeConverter(
        IToContractConverter<(RecipeId, IIngredient), Entities.Ingredient> ingredientConverter,
        IToContractConverter<(RecipeId, IPreparationStep), Entities.PreparationStep> preparationStepConverter)
    {
        _ingredientConverter = ingredientConverter;
        _preparationStepConverter = preparationStepConverter;
    }

    public Entities.Recipe ToContract(IRecipe source)
    {
        return new Entities.Recipe
        {
            Id = source.Id.Value,
            Name = source.Name.Value,
            Ingredients = source.Ingredients
                .Select(ing => _ingredientConverter.ToContract((source.Id, ing)))
                .ToList(),
            PreparationSteps = source.PreparationSteps
                .Select(step => _preparationStepConverter.ToContract((source.Id, step)))
                .ToList()
        };
    }
}