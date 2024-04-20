using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class TemporaryItemNotUpdateableReason : IReason
{
    public TemporaryItemNotUpdateableReason(ItemId id)
    {
        Message = $"Item {id} is temporary and thus cannot be updated.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.TemporaryItemNotUpdateable;
}