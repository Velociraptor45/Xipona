using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class AddItemToShoppingListContractConverter :
        IToContractConverter<AddItemToShoppingListRequest, AddItemToShoppingListContract>
    {
        private readonly ItemIdContractConverter _itemIdConverter;

        public AddItemToShoppingListContractConverter()
        {
            _itemIdConverter = new ItemIdContractConverter();
        }

        public AddItemToShoppingListContract ToContract(AddItemToShoppingListRequest request)
        {
            return new AddItemToShoppingListContract(
                _itemIdConverter.ToContract(request.ItemId),
                request.SectionId,
                request.Quantity);
        }
    }
}