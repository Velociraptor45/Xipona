using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;

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