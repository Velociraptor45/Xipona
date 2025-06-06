using Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class TemporaryShoppingListItemContractConverter :
    IToContractConverter<TemporaryShoppingListItemReadModel, TemporaryShoppingListItemContract>
{
    public TemporaryShoppingListItemContract ToContract(TemporaryShoppingListItemReadModel source)
    {
        return new TemporaryShoppingListItemContract(source.Id.Value, source.IsInBasket, source.QuantityInBasket.Value);
    }
}