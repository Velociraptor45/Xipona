using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

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