using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

public class RecipeTag : AggregateRoot, IRecipeTag
{
    public RecipeTag(RecipeTagId id, RecipeTagName name)
    {
        Id = id;
        Name = name;
    }

    public RecipeTagId Id { get; }
    public RecipeTagName Name { get; }
}