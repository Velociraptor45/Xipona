using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;

public class IngredientNotFoundReason : IReason
{
    public IngredientNotFoundReason(IngredientId ingredientId)
    {
        Message = $"Ingredient {ingredientId.Value} not found";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.IngredientNotFound;
}