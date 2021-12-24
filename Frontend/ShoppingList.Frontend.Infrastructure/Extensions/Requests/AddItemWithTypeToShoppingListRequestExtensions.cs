using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class AddItemWithTypeToShoppingListRequestExtensions
    {
        public static AddItemWithTypeToShoppingListContract ToContract(this AddItemWithTypeToShoppingListRequest request)
        {
            return new AddItemWithTypeToShoppingListContract
            {
                ShoppingListId = request.ShoppingListId,
                ItemId = request.ItemId,
                ItemTypeId = request.ItemTypeId,
                SectionId = request.SectionId,
                Quantity = request.Quantity
            };
        }
    }
}