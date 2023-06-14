using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class AddItemsToShoppingListsContractConverter
    : IToContractConverter<IEnumerable<AddToShoppingListItem>, AddItemsToShoppingListsContract>
{
    public AddItemsToShoppingListsContract ToContract(IEnumerable<AddToShoppingListItem> source)
    {
        var items = source.Select(i =>
            new AddItemToShoppingListContract(i.ItemId, i.ItemTypeId, i.SelectedStoreId, i.Quantity));

        return new AddItemsToShoppingListsContract(items);
    }
}