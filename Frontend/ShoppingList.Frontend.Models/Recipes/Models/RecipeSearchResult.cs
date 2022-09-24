using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class RecipeSearchResult
{
    public RecipeSearchResult(Guid recipeId, string name)
    {
        RecipeId = recipeId;
        Name = name;
    }

    public Guid RecipeId { get; }
    public string Name { get; }
}