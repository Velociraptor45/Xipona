namespace Xipona.Api.Domain.Common.Reasons;

public class ModelOutOfDateReason : IReason
{
    public string Message => "Saving failed because the version is out of date";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.ModelOutOfDate;
}