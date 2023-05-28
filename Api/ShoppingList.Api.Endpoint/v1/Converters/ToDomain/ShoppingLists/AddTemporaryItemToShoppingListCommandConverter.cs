using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class AddTemporaryItemToShoppingListCommandConverter
    : IToDomainConverter<(Guid, AddTemporaryItemToShoppingListContract), AddTemporaryItemToShoppingListCommand>
{
    public AddTemporaryItemToShoppingListCommand ToDomain((Guid, AddTemporaryItemToShoppingListContract) source)
    {
        (Guid shoppingListId, AddTemporaryItemToShoppingListContract? contract) = source;

        return new AddTemporaryItemToShoppingListCommand(
            new ShoppingListId(shoppingListId),
            new ItemName(contract.ItemName),
            contract.QuantityType.ToEnum<QuantityType>(),
            new QuantityInBasket(contract.Quantity),
            new Price(contract.Price),
            new SectionId(contract.SectionId),
            new TemporaryItemId(contract.TemporaryItemId));
    }
}