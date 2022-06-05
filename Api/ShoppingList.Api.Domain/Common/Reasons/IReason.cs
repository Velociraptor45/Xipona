namespace ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

public interface IReason
{
    string Message { get; }
    ErrorReasonCode ErrorCode { get; }
}