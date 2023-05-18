using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;

public class AddItemsToShoppingListsCommand : ICommand<bool>
{
    public AddItemsToShoppingListsCommand(IEnumerable<ItemToShoppingListAddition> itemToShoppingListAdditions)
    {
        ItemToShoppingListAdditions = itemToShoppingListAdditions.ToList();
    }

    public IReadOnlyCollection<ItemToShoppingListAddition> ItemToShoppingListAdditions { get; }
}