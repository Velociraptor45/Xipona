using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Reasons;

public class ItemWithTypesHasNoAvailabilitiesReason : IReason
{
    public ItemWithTypesHasNoAvailabilitiesReason(ItemId itemId)
    {
        Message =
            $"Item {itemId.Value} has types and thus no direct availabilities. Evaluate the availabilities on the individual types instead";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemWithTypesHasNoAvailabilities;
}