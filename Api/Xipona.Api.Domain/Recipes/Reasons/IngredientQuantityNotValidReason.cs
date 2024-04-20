using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

public class IngredientQuantityNotValidReason : IReason
{
    public IngredientQuantityNotValidReason(float quantity)
    {
        Message = $"Ingredient quantity must be greater 0 but is {quantity}";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.IngredientQuantityNotValid;
}