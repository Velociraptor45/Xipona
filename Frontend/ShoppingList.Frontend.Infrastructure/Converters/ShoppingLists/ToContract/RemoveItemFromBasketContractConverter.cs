using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class RemoveItemFromBasketContractConverter :
        IToContractConverter<RemoveItemFromBasketRequest, RemoveItemFromBasketContract>
    {
        private readonly IToContractConverter<ItemId, ItemIdContract> itemIdConverter;

        public RemoveItemFromBasketContractConverter(
            IToContractConverter<ItemId, ItemIdContract> itemIdConverter)
        {
            this.itemIdConverter = itemIdConverter;
        }

        public RemoveItemFromBasketContract ToContract(RemoveItemFromBasketRequest source)
        {
            return new RemoveItemFromBasketContract(
                itemIdConverter.ToContract(source.ItemId),
                source.ItemTypeId);
        }
    }
}