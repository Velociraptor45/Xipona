using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
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