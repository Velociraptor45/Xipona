using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;

public class RecipeConverter : IToContractConverter<IRecipe, Entities.Recipe>
{
    private readonly IToContractConverter<(RecipeId, IIngredient), Entities.Ingredient> _ingredientConverter;
    private readonly IToContractConverter<(RecipeId, IPreparationStep), Entities.PreparationStep> _preparationStepConverter;
    private readonly IToContractConverter<(RecipeId, RecipeTagId), TagsForRecipe> _tagsForRecipeConverter;

    public RecipeConverter(
        IToContractConverter<(RecipeId, IIngredient), Entities.Ingredient> ingredientConverter,
        IToContractConverter<(RecipeId, IPreparationStep), Entities.PreparationStep> preparationStepConverter,
        IToContractConverter<(RecipeId, RecipeTagId), TagsForRecipe> tagsForRecipeConverter)
    {
        _ingredientConverter = ingredientConverter;
        _preparationStepConverter = preparationStepConverter;
        _tagsForRecipeConverter = tagsForRecipeConverter;
    }

    public Entities.Recipe ToContract(IRecipe source)
    {
        return new Entities.Recipe
        {
            Id = source.Id,
            Name = source.Name,
            NumberOfServings = source.NumberOfServings,
            Ingredients = source.Ingredients
                .Select(ing => _ingredientConverter.ToContract((source.Id, ing)))
                .ToList(),
            PreparationSteps = source.PreparationSteps
                .Select(step => _preparationStepConverter.ToContract((source.Id, step)))
                .ToList(),
            Tags = source.Tags
                .Select(tag => _tagsForRecipeConverter.ToContract((source.Id, tag)))
                .ToList(),
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}