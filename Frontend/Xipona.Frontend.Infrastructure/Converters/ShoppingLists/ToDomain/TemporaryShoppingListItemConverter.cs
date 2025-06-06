using Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

public class TemporaryShoppingListItemConverter :
    IToDomainConverter<TemporaryShoppingListItemContract, TemporaryShoppingListItem>
{
    public TemporaryShoppingListItem ToDomain(TemporaryShoppingListItemContract source)
    {
        return new(source.ItemId, source.IsInBasket, source.QuantityInBasket);
    }
}