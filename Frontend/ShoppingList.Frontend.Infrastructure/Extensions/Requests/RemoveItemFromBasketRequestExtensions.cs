using ShoppingList.Api.Contracts.Commands.RemoveItemFromBasket;
using ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ShoppingList.Frontend.Models.Shared.Requests;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class RemoveItemFromBasketRequestExtensions
    {
        public static RemoveItemFromBasketContract ToContract(this RemoveItemFromBasketRequest request)
        {
            return new RemoveItemFromBasketContract()
            {
                ShoppingListId = request.ShoppingListId,
                ItemId = request.ItemId.ToContract()
            };
        }
    }
}