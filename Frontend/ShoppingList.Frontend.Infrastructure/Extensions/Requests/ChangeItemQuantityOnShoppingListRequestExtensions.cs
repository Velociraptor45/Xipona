using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class ChangeItemQuantityOnShoppingListRequestExtensions
    {
        public static ChangeItemQuantityOnShoppingListContract ToContract(this ChangeItemQuantityOnShoppingListRequest request)
        {
            return new ChangeItemQuantityOnShoppingListContract
            {
                ShoppingListId = request.ShoppingListId,
                ItemId = request.ItemId.ToContract(),
                ItemTypeId = request.ItemTypeId,
                Quantity = request.Quantity
            };
        }
    }
}