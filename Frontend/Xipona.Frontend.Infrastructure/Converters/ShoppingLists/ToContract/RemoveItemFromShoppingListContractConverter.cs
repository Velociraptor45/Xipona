using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

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