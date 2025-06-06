using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Services.Searches;

public class ItemWithMatchingItemTypeIds
{
    public ItemWithMatchingItemTypeIds(IItem item, IEnumerable<ItemTypeId> matchingItemTypeIds)
    {
        Item = item;
        MatchingItemTypeIds = matchingItemTypeIds.ToList();
    }

    public IItem Item { get; }
    public IReadOnlyCollection<ItemTypeId> MatchingItemTypeIds { get; }
}