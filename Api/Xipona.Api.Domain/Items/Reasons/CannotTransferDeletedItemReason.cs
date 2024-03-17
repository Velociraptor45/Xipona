using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotTransferDeletedItemReason : IReason
{
    public CannotTransferDeletedItemReason(ItemId id)
    {
        Message = $"Cannot transfer deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotTransferDeletedItem;
}