using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
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