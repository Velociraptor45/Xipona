using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
public record struct Price
{
    public Price()
    {
        throw new NotSupportedException("Empty constructor is for price not supported.");
    }

    public Price(float value)
    {
        Value = value;

        if (value <= 0)
            throw new DomainException(new PriceNotValidReason());
    }

    public float Value { get; }
}