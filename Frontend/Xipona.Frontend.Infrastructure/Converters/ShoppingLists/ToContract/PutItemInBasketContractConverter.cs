using Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class PutItemInBasketContractConverter :
    IToContractConverter<PutItemInBasketRequest, PutItemInBasketContract>
{
    private readonly IToContractConverter<ShoppingListItemId, ItemIdContract> _itemIdConverter;

    public PutItemInBasketContractConverter(
        IToContractConverter<ShoppingListItemId, ItemIdContract> itemIdConverter)
    {
        _itemIdConverter = itemIdConverter;
    }

    public PutItemInBasketContract ToContract(PutItemInBasketRequest source)
    {
        return new PutItemInBasketContract(
            _itemIdConverter.ToContract(source.ItemId),
            source.ItemTypeId);
    }
}