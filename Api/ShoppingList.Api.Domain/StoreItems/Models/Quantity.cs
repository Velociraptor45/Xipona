using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public class Quantity : GenericPrimitive<float>
{
    public Quantity(float value) : base(value)
    {
    }
}