using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListSectionContract
    {
        public ShoppingListSectionContract(Guid id, string name, int sortingIndex, bool isDefaultSection,
            IEnumerable<ShoppingListItemContract> items)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
            Items = items.ToList();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SortingIndex { get; set; }
        public bool IsDefaultSection { get; set; }
        public IReadOnlyCollection<ShoppingListItemContract> Items { get; }
    }
}