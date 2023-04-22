using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class AddItemsToShoppingListsCommandConverter :
    IToDomainConverter<AddItemsToShoppingListsContract, AddItemsToShoppingListsCommand>
{
    public AddItemsToShoppingListsCommand ToDomain(AddItemsToShoppingListsContract source)
    {
        return new AddItemsToShoppingListsCommand(source.Items
            .Select(i => new ItemToShoppingListAddition(
                new ItemId(i.ItemId),
                i.ItemTypeId is null ? null : new ItemTypeId(i.ItemTypeId.Value),
                new StoreId(i.StoreId),
                new QuantityInBasket(i.Quantity))));
    }
}