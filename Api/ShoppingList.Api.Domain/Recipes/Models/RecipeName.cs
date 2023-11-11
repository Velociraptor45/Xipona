using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
public record RecipeName : Name
{
    public RecipeName(string value) : base(value)
    {
    }
}