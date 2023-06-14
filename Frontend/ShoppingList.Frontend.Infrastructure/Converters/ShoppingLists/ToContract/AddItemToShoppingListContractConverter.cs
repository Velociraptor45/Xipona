using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class AddItemToShoppingListContractConverter :
        IToContractConverter<AddItemToShoppingListRequest, AddItemToShoppingListContract>
    {
        public AddItemToShoppingListContract ToContract(AddItemToShoppingListRequest request)
        {
            return new AddItemToShoppingListContract(
                request.ItemId,
                request.SectionId,
                request.Quantity);
        }
    }
}