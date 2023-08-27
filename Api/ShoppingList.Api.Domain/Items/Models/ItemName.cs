using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;
public record ItemName : Name
{
    public ItemName(string value) : base(value)
    {
    }
}