using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

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