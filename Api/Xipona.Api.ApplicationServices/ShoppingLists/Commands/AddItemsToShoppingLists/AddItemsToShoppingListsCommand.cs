using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ShoppingLists.Services.AddItems;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;

public class AddItemsToShoppingListsCommand : ICommand<bool>
{
    public AddItemsToShoppingListsCommand(IEnumerable<ItemToShoppingListAddition> itemToShoppingListAdditions)
    {
        ItemToShoppingListAdditions = itemToShoppingListAdditions.ToList();
    }

    public IReadOnlyCollection<ItemToShoppingListAddition> ItemToShoppingListAdditions { get; }
}