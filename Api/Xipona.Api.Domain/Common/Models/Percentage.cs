using System.Globalization;

namespace ProjectHermes.Xipona.Api.Domain.Common.Models;

public readonly record struct Percentage
{
    public Percentage() : this(0)
    {
        throw new NotSupportedException("An empty percentage is not allowed. Use the other ctor.");
    }

    public Percentage(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public Percentage Inverted => new(100 - Value);

    public static implicit operator decimal(Percentage percentage)
    {
        return percentage.Value;
    }

    public static decimal operator *(decimal left, Percentage right)
    {
        return left * right.Value;
    }

    public static decimal operator *(Percentage left, decimal right)
    {
        return left.Value * right;
    }

    public static decimal operator *(Percentage left, Percentage right)
    {
        return left.Value * right.Value;
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}