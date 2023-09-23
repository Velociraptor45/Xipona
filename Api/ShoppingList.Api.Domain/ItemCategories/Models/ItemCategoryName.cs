using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

public record ItemCategoryName : Name
{
    public ItemCategoryName(string value) : base(value)
    {
    }
}