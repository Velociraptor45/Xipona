using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public record SectionName : Name
{
    public SectionName(string value) : base(value)
    {
    }
}