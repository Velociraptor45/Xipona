using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class CannotCreateItemTypeWithoutAvailabilitiesReason : IReason
{
    public CannotCreateItemTypeWithoutAvailabilitiesReason()
    {
        Message = "Cannot create item type without availabilities";
    }
    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotCreateItemTypeWithoutAvailabilities;
}