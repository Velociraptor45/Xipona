using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class TemporaryShoppingListItemContractConverter :
    IToContractConverter<TemporaryShoppingListItemReadModel, TemporaryShoppingListItemContract>
{
    public TemporaryShoppingListItemContract ToContract(TemporaryShoppingListItemReadModel source)
    {
        return new TemporaryShoppingListItemContract(source.Id.Value, source.IsInBasket, source.QuantityInBasket.Value);
    }
}