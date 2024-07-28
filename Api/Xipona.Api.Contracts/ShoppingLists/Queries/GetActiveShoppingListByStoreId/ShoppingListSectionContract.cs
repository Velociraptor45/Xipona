using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId
{
    /// <summary>
    /// Represents a shopping list section.
    /// </summary>
    public class ShoppingListSectionContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sortingIndex"></param>
        /// <param name="isDefaultSection"></param>
        /// <param name="items"></param>
        public ShoppingListSectionContract(Guid id, string name, int sortingIndex, bool isDefaultSection,
            IEnumerable<ShoppingListItemContract> items)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
            Items = items.ToList();
        }

        /// <summary>
        /// The ID of the section.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the section.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sorting index of the section.
        /// </summary>
        public int SortingIndex { get; set; }

        /// <summary>
        /// Whether the section is the store's default section.
        /// </summary>
        public bool IsDefaultSection { get; set; }

        /// <summary>
        /// The items in the section.
        /// </summary>
        public IReadOnlyCollection<ShoppingListItemContract> Items { get; }
    }
}