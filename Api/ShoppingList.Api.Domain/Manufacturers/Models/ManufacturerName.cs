using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

public record ManufacturerName : Name
{
    public ManufacturerName(string value) : base(value)
    {
    }
}