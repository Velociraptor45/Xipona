using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.RecipeById;

public class RecipeByIdQuery : IQuery<IRecipe>
{
    public RecipeByIdQuery(RecipeId recipeId)
    {
        RecipeId = recipeId;
    }

    public RecipeId RecipeId { get; }
}