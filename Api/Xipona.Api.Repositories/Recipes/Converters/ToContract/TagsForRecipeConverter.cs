using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToContract;

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