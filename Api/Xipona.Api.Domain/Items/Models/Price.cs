using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;
public readonly record struct Price
{
    public Price()
    {
        throw new NotSupportedException("Empty constructor is for price not supported.");
    }

    public Price(decimal value)
    {
        Value = value;

        if (value <= 0)
            throw new DomainException(new PriceNotValidReason());
    }

    public decimal Value { get; }

    public static implicit operator decimal(Price price)
    {
        return price.Value;
    }

    public override string ToString()
    {
        return Value.ToString("##,###");
    }
}