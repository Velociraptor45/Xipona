using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;
public readonly record struct Price
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

    public static implicit operator float(Price price)
    {
        return price.Value;
    }

    public override string ToString()
    {
        return Value.ToString("##,###");
    }
}