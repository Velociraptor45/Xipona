using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;

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