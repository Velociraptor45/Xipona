namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

public interface IReason
{
    string Message { get; }
    ErrorReasonCode ErrorCode { get; }
}