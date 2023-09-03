using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

public record RecipeTagName : Name
{
    public RecipeTagName(string value) : base(value)
    {
    }
}