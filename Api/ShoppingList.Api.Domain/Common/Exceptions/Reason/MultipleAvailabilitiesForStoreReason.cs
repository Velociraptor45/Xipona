namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class MultipleAvailabilitiesForStoreReason : IReason
    {
        public string Message => "Multiple availabilities for one store were provided.";

        public ErrorReasonCode ErrorCode => ErrorReasonCode.MultipleAvailabilitiesForStore;
    }
}