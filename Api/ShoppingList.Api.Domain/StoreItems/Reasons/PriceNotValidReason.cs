using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

public class PriceNotValidReason : IReason
{
    public PriceNotValidReason()
    {
        Message = "Price must be greater 0.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.PriceNotValid;
}