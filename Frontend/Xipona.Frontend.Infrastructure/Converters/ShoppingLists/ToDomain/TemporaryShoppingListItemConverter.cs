using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

public class TemporaryShoppingListItemConverter :
    IToDomainConverter<TemporaryShoppingListItemContract, TemporaryShoppingListItem>
{
    public TemporaryShoppingListItem ToDomain(TemporaryShoppingListItemContract source)
    {
        return new(source.ItemId, source.IsInBasket, source.QuantityInBasket);
    }
}