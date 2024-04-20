using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class TemporaryItemCannotHaveTypeIdReason : IReason
{
    public TemporaryItemCannotHaveTypeIdReason()
    {
        Message = "A temporary item cannot have an item type id.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.TemporaryItemCannotHaveTypeId;
}