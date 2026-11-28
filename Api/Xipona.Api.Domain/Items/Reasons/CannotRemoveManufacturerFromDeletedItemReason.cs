using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class CannotRemoveManufacturerFromDeletedItemReason : IReason
{
    public CannotRemoveManufacturerFromDeletedItemReason(ItemId id)
    {
        Message = $"Cannot remove manufacturer from deleted item ({id.Value})";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotRemoveManufacturerFromDeletedItem;
}