using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class PutItemInBasketContractConverter :
        IToContractConverter<PutItemInBasketRequest, PutItemInBasketContract>
    {
        private readonly IToContractConverter<ItemId, ItemIdContract> itemIdConverter;

        public PutItemInBasketContractConverter(
            IToContractConverter<ItemId, ItemIdContract> itemIdConverter)
        {
            this.itemIdConverter = itemIdConverter;
        }

        public PutItemInBasketContract ToContract(PutItemInBasketRequest source)
        {
            return new PutItemInBasketContract()
            {
                ShoppingListId = source.ShoppingListId,
                ItemId = itemIdConverter.ToContract(source.ItemId),
                ItemTypeId = source.ItemTypeId
            };
        }
    }
}