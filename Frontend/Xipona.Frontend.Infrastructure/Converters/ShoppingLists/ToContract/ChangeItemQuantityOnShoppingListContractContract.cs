using Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class ChangeItemQuantityOnShoppingListContractContract :
    IToContractConverter<ChangeItemQuantityOnShoppingListRequest, ChangeItemQuantityOnShoppingListContract>
{
    private readonly IToContractConverter<ShoppingListItemId, ItemIdContract> _itemIdConverter;

    public ChangeItemQuantityOnShoppingListContractContract(
        IToContractConverter<ShoppingListItemId, ItemIdContract> itemIdConverter)
    {
        _itemIdConverter = itemIdConverter;
    }

    public ChangeItemQuantityOnShoppingListContract ToContract(ChangeItemQuantityOnShoppingListRequest source)
    {
        return new ChangeItemQuantityOnShoppingListContract(
            _itemIdConverter.ToContract(source.ItemId),
            source.ItemTypeId,
            source.Quantity);
    }
}