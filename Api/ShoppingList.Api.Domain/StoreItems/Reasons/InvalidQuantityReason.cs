using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

public class InvalidQuantityReason : IReason
{
    public InvalidQuantityReason(float quantity)
    {
        Message = $"Quantity must be greater than 0 but was {quantity}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.InvalidQuantity;
}