using ShoppingList.Api.Contracts.Commands.ChangeItemQuantityOnShoppingList;
using ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ShoppingList.Frontend.Models.Shared.Requests;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class ChangeItemQuantityOnShoppingListRequestExtensions
    {
        public static ChangeItemQuantityOnShoppingListContract ToContract(this ChangeItemQuantityOnShoppingListRequest request)
        {
            return new ChangeItemQuantityOnShoppingListContract
            {
                ShoppingListId = request.ShoppingListId,
                ItemId = request.ItemId.ToContract(),
                Quantity = request.Quantity
            };
        }
    }
}