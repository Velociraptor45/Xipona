namespace ProjectHermes.Xipona.Api.Domain.Common.Reasons;

public class ModelOutOfDateReason : IReason
{
    public ModelOutOfDateReason()
    {
        Message = "Saving failed because the version is out of date";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.ModelOutOfDate;
}