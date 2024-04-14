using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class RemoveItemFromBasketContractConverter :
        IToContractConverter<RemoveItemFromBasketRequest, RemoveItemFromBasketContract>
    {
        private readonly IToContractConverter<ShoppingListItemId, ItemIdContract> _itemIdConverter;

        public RemoveItemFromBasketContractConverter(
            IToContractConverter<ShoppingListItemId, ItemIdContract> itemIdConverter)
        {
            _itemIdConverter = itemIdConverter;
        }

        public RemoveItemFromBasketContract ToContract(RemoveItemFromBasketRequest source)
        {
            return new RemoveItemFromBasketContract(
                _itemIdConverter.ToContract(source.ItemId),
                source.ItemTypeId);
        }
    }
}