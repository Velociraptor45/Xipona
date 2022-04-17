using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public record struct Quantity
{
    public Quantity()
    {
        throw new NotSupportedException("Empty constructor for Quantity not supported.");
    }

    public Quantity(float value)
    {
        Value = value;

        if (Value <= 0)
            throw new DomainException(new InvalidQuantityReason(Value));
    }

    public float Value { get; }
}