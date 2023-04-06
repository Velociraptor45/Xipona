using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

public class RecipeTag : AggregateRoot, IRecipeTag
{
    public RecipeTag(RecipeTagId id, string name)
    {
        Id = id;
        Name = name;
    }

    public RecipeTagId Id { get; }
    public string Name { get; }
}