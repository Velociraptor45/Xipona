using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class RemoveItemFromShoppingListContractConverter :
    IToContractConverter<RemoveItemFromShoppingListRequest, RemoveItemFromShoppingListContract>
{
    private readonly ItemIdContractConverter _itemIdConverter;

    public RemoveItemFromShoppingListContractConverter()
    {
        _itemIdConverter = new ItemIdContractConverter();
    }

    public RemoveItemFromShoppingListContract ToContract(RemoveItemFromShoppingListRequest request)
    {
        return new RemoveItemFromShoppingListContract(
            _itemIdConverter.ToContract(request.ItemId),
            request.ItemTypeId);
    }
}