using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;

public class DummyReason : IReason
{
    public DummyReason(string message, ErrorReasonCode errorCode)
    {
        Message = message;
        ErrorCode = errorCode;
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode { get; }
}