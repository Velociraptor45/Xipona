using System;
using System.Collections.Generic;
using System.Linq;

namespace Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId
{
    /// <summary>
    /// Represents a shopping list.
    /// </summary>
    public class ShoppingListContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="store"></param>
        /// <param name="sections"></param>
        /// <param name="completionDate"></param>
        /// <param name="shoppingListDiscounts"></param>
        public ShoppingListContract(Guid id, ShoppingListStoreContract store,
            IEnumerable<ShoppingListSectionContract> sections, DateTimeOffset? completionDate,
            IEnumerable<ShoppingListDiscountContract> shoppingListDiscounts)
        {
            Id = id;
            Store = store;
            Sections = sections.ToList();
            CompletionDate = completionDate;
            ShoppingListDiscounts = shoppingListDiscounts.ToList();
        }

        /// <summary>
        /// The ID of the shopping list.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The store the shopping list targets.
        /// </summary>
        public ShoppingListStoreContract Store { get; }

        /// <summary>
        /// The store's sections. Only contains sections where the list has items.
        /// </summary>
        public IReadOnlyCollection<ShoppingListSectionContract> Sections { get; }

        /// <summary>
        /// The date the shopping list was completed.
        /// Null if the list is active.
        /// </summary>
        public DateTimeOffset? CompletionDate { get; }

        /// <summary>
        /// The discounts applied to the entire shopping list.
        /// </summary>
        public IReadOnlyCollection<ShoppingListDiscountContract> ShoppingListDiscounts { get; }
    }
}