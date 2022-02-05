namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class InvalidItemQuantityReason : IReason
{
    public InvalidItemQuantityReason(float quantity)
    {
        Message = $"Item quantity must be greater than 0 but was {quantity}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.InvalidItemQuantity;
}