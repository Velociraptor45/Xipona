using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

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