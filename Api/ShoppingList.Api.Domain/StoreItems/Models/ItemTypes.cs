using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class ItemTypes : IReadOnlyCollection<IItemType>
    {
        private readonly Dictionary<ItemTypeId, IItemType> _itemTypes;

        public ItemTypes(IEnumerable<IItemType> itemTypes)
        {
            _itemTypes = itemTypes.ToDictionary(t => t.Id);
        }

        public int Count => _itemTypes.Count;

        public IEnumerator<IItemType> GetEnumerator()
        {
            return _itemTypes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IReadOnlyCollection<IItemType> GetForStore(StoreId storeId)
        {
            return _itemTypes.Values
                .Where(t => t.Availabilities.Any(av => av.StoreId == storeId))
                .ToList()
                .AsReadOnly();
        }

        public bool ContainsId(ItemTypeId itemTypeId)
        {
            return _itemTypes.ContainsKey(itemTypeId);
        }

        public bool TryGetValue(ItemTypeId id, out IItemType? itemType)
        {
            return _itemTypes.TryGetValue(id, out itemType);
        }
    }
}