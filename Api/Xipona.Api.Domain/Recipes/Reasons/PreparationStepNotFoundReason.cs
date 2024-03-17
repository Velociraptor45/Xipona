using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

public class PreparationStepNotFoundReason : IReason
{
    public PreparationStepNotFoundReason(PreparationStepId preparationStepId)
    {
        Message = $"Preparation step {preparationStepId.Value} not found";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.PreparationStepNotFound;
}