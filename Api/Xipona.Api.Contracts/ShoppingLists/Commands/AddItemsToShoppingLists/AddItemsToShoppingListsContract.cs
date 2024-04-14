using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists
{
    public class AddItemsToShoppingListsContract
    {
        public AddItemsToShoppingListsContract(IEnumerable<AddItemToShoppingListContract> items)
        {
            Items = items;
        }

        public IEnumerable<AddItemToShoppingListContract> Items { get; set; }
    }
}