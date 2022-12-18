using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
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
}