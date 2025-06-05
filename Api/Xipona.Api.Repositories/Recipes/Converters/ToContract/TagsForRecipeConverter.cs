using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Repositories.Recipes.Entities;

namespace Xipona.Api.Repositories.Recipes.Converters.ToContract;

public class TagsForRecipeConverter : IToContractConverter<(RecipeId, RecipeTagId), TagsForRecipe>
{
    public TagsForRecipe ToContract((RecipeId, RecipeTagId) source)
    {
        (RecipeId recipeId, RecipeTagId recipeTagId) = source;
        return new TagsForRecipe
        {
            RecipeId = recipeId.Value,
            RecipeTagId = recipeTagId.Value
        };
    }
}