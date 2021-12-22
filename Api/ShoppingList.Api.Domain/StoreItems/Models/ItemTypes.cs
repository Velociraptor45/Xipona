using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class ItemTypes : IReadOnlyCollection<IItemType>
    {
        private readonly List<IItemType> _itemTypes;

        public ItemTypes(IEnumerable<IItemType> itemTypes)
        {
            _itemTypes = itemTypes.ToList();
        }

        public int Count => _itemTypes.Count;

        public IEnumerator<IItemType> GetEnumerator()
        {
            return _itemTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IReadOnlyCollection<IItemType> GetForStore(StoreId storeId)
        {
            return _itemTypes
                .Where(t => t.Availabilities.Any(av => av.StoreId == storeId))
                .ToList()
                .AsReadOnly();
        }
    }
}