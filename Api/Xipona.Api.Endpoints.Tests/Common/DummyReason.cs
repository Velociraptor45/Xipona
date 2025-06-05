using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Endpoints.Tests.Common;

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