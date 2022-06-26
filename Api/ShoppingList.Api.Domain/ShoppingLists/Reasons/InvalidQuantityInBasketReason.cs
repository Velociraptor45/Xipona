using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;

public class InvalidQuantityInBasketReason : IReason
{
    public InvalidQuantityInBasketReason(float quantity)
    {
        Message = $"Quantity in basket must be greater than 0 but was {quantity}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.InvalidQuantityInBasket;
}