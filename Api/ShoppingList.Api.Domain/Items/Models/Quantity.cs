using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public readonly record struct Quantity
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

    public static implicit operator float(Quantity quantity)
    {
        return quantity.Value;
    }

    public override string ToString()
    {
        return Value.ToString("##.###");
    }
}