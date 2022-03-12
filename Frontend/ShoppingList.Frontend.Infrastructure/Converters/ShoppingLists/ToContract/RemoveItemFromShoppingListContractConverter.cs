using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class RemoveItemFromShoppingListContractConverter
    {
        private readonly ItemIdContractConverter _itemIdConverter;

        public RemoveItemFromShoppingListContractConverter()
        {
            _itemIdConverter = new ItemIdContractConverter();
        }

        public RemoveItemFromShoppingListContract ToContract(RemoveItemFromShoppingListRequest request)
        {
            return new RemoveItemFromShoppingListContract
            {
                ShoppingListId = request.ShoppingListId,
                ItemId = _itemIdConverter.ToContract(request.ItemId),
                ItemTypeId = request.ItemTypeId
            };
        }
    }
}