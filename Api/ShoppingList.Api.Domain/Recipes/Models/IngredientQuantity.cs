using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
public readonly record struct IngredientQuantity
{
    public IngredientQuantity(float value)
    {
        if (value <= 0)
            throw new DomainException(new IngredientQuantityNotValidReason(value));

        Value = value;
    }
    public float Value { get; }
}