using ShoppingList.Api.Contracts.Commands.AddItemToShoppingList;
using ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ShoppingList.Frontend.Models.Shared.Requests;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class AddItemToShoppingListRequestExtensions
    {
        public static AddItemToShoppingListContract ToContract(this AddItemToShoppingListRequest contract)
        {
            return new AddItemToShoppingListContract
            {
                ShoppingListId = contract.ShoppingListId,
                ItemId = contract.ItemId.ToContract(),
                Quantity = contract.Quantity
            };
        }
    }
}