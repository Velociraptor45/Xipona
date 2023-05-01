using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;
public class NumberOfServingsMustBeAtLeastOneReason : IReason
{
    public string Message => "Number of servings must be at least one";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.NumberOfServingsMustBeAtLeastOne;
}
