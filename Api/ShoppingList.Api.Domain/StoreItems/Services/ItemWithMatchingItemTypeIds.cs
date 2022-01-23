using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services
{
    public class ItemWithMatchingItemTypeIds
    {
        public ItemWithMatchingItemTypeIds(IStoreItem item, IEnumerable<ItemTypeId> matchingItemTypeIds)
        {
            Item = item;
            MatchingItemTypeIds = matchingItemTypeIds.ToList();
        }

        public IStoreItem Item { get; }
        public IReadOnlyCollection<ItemTypeId> MatchingItemTypeIds { get; }
    }
}