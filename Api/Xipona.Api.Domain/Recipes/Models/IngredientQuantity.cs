using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;
public readonly record struct IngredientQuantity
{
    public IngredientQuantity(float value)
    {
        if (value <= 0)
            throw new DomainException(new IngredientQuantityNotValidReason(value));

        Value = value;
    }
    public float Value { get; }

    public override string ToString()
    {
        return Value.ToString("##,###");
    }
}