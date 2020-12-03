using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
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