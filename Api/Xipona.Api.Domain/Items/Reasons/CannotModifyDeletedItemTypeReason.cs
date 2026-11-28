using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class CannotModifyDeletedItemTypeReason : IReason
{
    public CannotModifyDeletedItemTypeReason(ItemTypeId id)
    {
        Message = $"Cannot modify deleted item type {id.Value}";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotModifyDeletedItemType;
}