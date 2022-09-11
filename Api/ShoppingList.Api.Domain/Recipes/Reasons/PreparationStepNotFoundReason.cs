using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;

public class PreparationStepNotFoundReason : IReason
{
    public PreparationStepNotFoundReason(PreparationStepId preparationStepId)
    {
        Message = $"Preparation step {preparationStepId.Value} not found";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.PreparationStepNotFound;
}