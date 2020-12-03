using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ShoppingList.Frontend.Models.Shared.Requests;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class RemoveItemFromShoppingListRequestExtensions
    {
        public static RemoveItemFromShoppingListContract ToContract(this RemoveItemFromShoppingListRequest request)
        {
            return new RemoveItemFromShoppingListContract
            {
                ShoppingListId = request.ShoppingListId,
                ItemId = request.ItemId.ToContract()
            };
        }
    }
}