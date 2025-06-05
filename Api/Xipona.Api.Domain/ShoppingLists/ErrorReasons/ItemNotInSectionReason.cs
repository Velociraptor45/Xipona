using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.ErrorReasons;

public class ItemNotInSectionReason : IReason
{
    public ItemNotInSectionReason(ItemId shoppingListItemId, SectionId sectionId)
    {
        Message = $"Item {shoppingListItemId} isn't in section {sectionId}";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ItemNotInSection;
}