using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ShoppingList.Frontend.Redux.Recipes.States;

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