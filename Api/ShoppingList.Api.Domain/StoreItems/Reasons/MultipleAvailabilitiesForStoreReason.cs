using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

public class MultipleAvailabilitiesForStoreReason : IReason
{
    public string Message => "Multiple availabilities for one store were provided.";

    public ErrorReasonCode ErrorCode => ErrorReasonCode.MultipleAvailabilitiesForStore;
}