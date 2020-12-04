using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class PutItemInBasketRequestExtensions
    {
        public static PutItemInBasketContract ToContract(this PutItemInBasketRequest request)
        {
            return new PutItemInBasketContract()
            {
                ShopingListId = request.ShoppingListId,
                ItemId = request.ItemId.ToContract()
            };
        }
    }
}