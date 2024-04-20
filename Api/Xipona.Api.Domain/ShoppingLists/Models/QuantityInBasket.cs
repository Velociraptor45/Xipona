using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
public readonly record struct QuantityInBasket
{
    public QuantityInBasket()
    {
        throw new NotSupportedException("Empty constructor is not supported for QuantityInBasket.");
    }

    public QuantityInBasket(float value)
    {
        Value = value;

        if (Value <= 0f)
            throw new DomainException(new InvalidQuantityInBasketReason(value));
    }

    public float Value { get; }

    public static QuantityInBasket operator +(QuantityInBasket a, QuantityInBasket b)
    {
        return new QuantityInBasket(a.Value + b.Value);
    }

    public static QuantityInBasket operator -(QuantityInBasket a, QuantityInBasket b)
    {
        return new QuantityInBasket(a.Value - b.Value);
    }

    public static QuantityInBasket operator *(QuantityInBasket a, QuantityInBasket b)
    {
        return new QuantityInBasket(a.Value * b.Value);
    }

    public static QuantityInBasket operator /(QuantityInBasket a, QuantityInBasket b)
    {
        return new QuantityInBasket(a.Value / b.Value);
    }

    public override string ToString()
    {
        return Value.ToString("##,###");
    }
}