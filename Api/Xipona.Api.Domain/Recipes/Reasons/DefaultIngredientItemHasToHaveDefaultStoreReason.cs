using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

public class DefaultIngredientItemHasToHaveDefaultStoreReason : IReason
{
    public string Message => "You have to specify a default store for the default item of an ingredient";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.DefaultIngredientItemHasToHaveDefaultStore;
}