using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class AddItemToShoppingListRequestExtensions
    {
        public static AddItemToShoppingListContract ToContract(this AddItemToShoppingListRequest request)
        {
            return new AddItemToShoppingListContract
            {
                ShoppingListId = request.ShoppingListId,
                ItemId = request.ItemId.ToContract(),
                SectionId = request.SectionId,
                Quantity = request.Quantity
            };
        }
    }
}