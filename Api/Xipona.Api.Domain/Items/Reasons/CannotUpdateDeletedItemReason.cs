using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class CannotUpdateDeletedItemReason : IReason
{
    public CannotUpdateDeletedItemReason(ItemId id)
    {
        Message = $"Cannot update deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotUpdateDeletedItem;
}