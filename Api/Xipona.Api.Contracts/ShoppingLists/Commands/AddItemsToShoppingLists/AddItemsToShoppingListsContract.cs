using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists
{
    /// <summary>
    /// Represents a contract to add multiple items to multiple shopping lists.
    /// </summary>
    public class AddItemsToShoppingListsContract
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        public AddItemsToShoppingListsContract(IEnumerable<AddItemToShoppingListContract> items)
        {
            Items = items;
        }

        /// <summary>
        /// The items to add to the shopping lists.
        /// </summary>
        public IEnumerable<AddItemToShoppingListContract> Items { get; set; }
    }
}