using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class AddItemWithTypeToShoppingListContractConverter :
    IToContractConverter<AddItemWithTypeToShoppingListRequest, AddItemWithTypeToShoppingListContract>
{
    public AddItemWithTypeToShoppingListContract ToContract(AddItemWithTypeToShoppingListRequest source)
    {
        return new AddItemWithTypeToShoppingListContract(
            source.SectionId,
            source.Quantity);
    }
}