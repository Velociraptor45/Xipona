using Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Services.AddItems;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

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