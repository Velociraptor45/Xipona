using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;
public readonly record struct NumberOfServings
{
    public NumberOfServings()
    {
        throw new NotSupportedException("Empty constructor for NumberOfServings not supported");
    }

    public NumberOfServings(int value)
    {
        if (value < 1)
            throw new DomainException(new NumberOfServingsMustBeAtLeastOneReason());

        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(NumberOfServings numberOfServings)
    {
        return numberOfServings.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}