namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class TemporaryItemCannotHaveTypeIdReason : IReason
{
    public TemporaryItemCannotHaveTypeIdReason()
    {
        Message = "A temporary item cannot have an item type id.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.TemporaryItemCannotHaveTypeIdReason;
}