using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

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