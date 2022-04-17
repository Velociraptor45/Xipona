using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Reason;

public interface IReason
{
    string Message { get; }
    ErrorReasonCode ErrorCode { get; }
}