using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.AddItems;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;

public class AddItemsToShoppingListsCommand : ICommand<bool>
{
    public AddItemsToShoppingListsCommand(IEnumerable<ItemToShoppingListAddition> itemToShoppingListAdditions)
    {
        ItemToShoppingListAdditions = itemToShoppingListAdditions.ToList();
    }

    public IReadOnlyCollection<ItemToShoppingListAddition> ItemToShoppingListAdditions { get; }
}