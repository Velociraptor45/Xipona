using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public record StoreName : Name
{
    public StoreName(string value) : base(value)
    {
    }
}