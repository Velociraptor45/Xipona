namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class InvalidQuantityReason : IReason
{
    public InvalidQuantityReason(float quantity)
    {
        Message = $"Quantity must be greater than 0 but was {quantity}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.InvalidQuantity;
}