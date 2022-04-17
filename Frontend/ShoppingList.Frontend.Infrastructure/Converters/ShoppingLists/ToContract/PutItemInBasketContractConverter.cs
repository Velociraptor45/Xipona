using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
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
}