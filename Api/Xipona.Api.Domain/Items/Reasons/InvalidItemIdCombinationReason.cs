using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Domain.Items.Reasons;

public class InvalidItemIdCombinationReason : IReason
{
    public string Message => "You cannot specify an item type id without an item id";
    public ErrorReasonCode ErrorCode => ErrorReasonCode.InvalidItemIdCombination;
}