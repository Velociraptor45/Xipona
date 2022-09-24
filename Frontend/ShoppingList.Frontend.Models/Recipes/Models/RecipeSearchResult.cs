using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class RecipeSearchResult : ISearchResult
{
    public RecipeSearchResult(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}