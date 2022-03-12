using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListSectionContract
    {
        private readonly IEnumerable<ShoppingListItemContract> _items;

        public ShoppingListSectionContract(Guid id, string name, int sortingIndex, bool isDefaultSection,
            IEnumerable<ShoppingListItemContract> items)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
            _items = items;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SortingIndex { get; set; }
        public bool IsDefaultSection { get; set; }
        public IReadOnlyCollection<ShoppingListItemContract> Items => _items.ToList().AsReadOnly();
    }
}