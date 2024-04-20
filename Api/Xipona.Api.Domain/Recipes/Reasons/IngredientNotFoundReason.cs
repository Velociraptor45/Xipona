using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

public class IngredientNotFoundReason : IReason
{
    public IngredientNotFoundReason(IngredientId ingredientId)
    {
        Message = $"Ingredient {ingredientId.Value} not found";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.IngredientNotFound;
}