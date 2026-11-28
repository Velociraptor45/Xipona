using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

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