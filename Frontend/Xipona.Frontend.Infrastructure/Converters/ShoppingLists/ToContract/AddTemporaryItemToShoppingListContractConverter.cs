using Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class AddTemporaryItemToShoppingListContractConverter
    : IToContractConverter<AddTemporaryItemToShoppingListRequest, AddTemporaryItemToShoppingListContract>
{
    public AddTemporaryItemToShoppingListContract ToContract(AddTemporaryItemToShoppingListRequest source)
    {
        return new AddTemporaryItemToShoppingListContract(
            source.ItemName,
            source.QuantityType,
            source.Quantity,
            source.Price,
            source.SectionId,
            source.TemporaryId);
    }
}