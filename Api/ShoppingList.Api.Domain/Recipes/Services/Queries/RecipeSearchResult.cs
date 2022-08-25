using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public class RecipeSearchResult
{
    public RecipeSearchResult(RecipeId id, RecipeName name)
    {
        Id = id;
        Name = name;
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; }
}