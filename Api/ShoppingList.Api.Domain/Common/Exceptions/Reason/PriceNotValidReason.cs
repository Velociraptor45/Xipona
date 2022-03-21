namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public class PriceNotValidReason : IReason
{
    public PriceNotValidReason()
    {
        Message = "Price must be greater 0.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.PriceNotValid;
}