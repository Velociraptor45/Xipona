using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Reasons;

public class ItemNotTemporaryReason : IReason
{
    public ItemNotTemporaryReason(ItemId id)
    {
        Message = $"Item {id} is not temporary.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotTemporary;
}