using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemSectionIdExtensions
    {
        public static ShoppingListSectionId ToShoppingListSectionId(this StoreItemSectionId id)
        {
            return new ShoppingListSectionId(id.Value);
        }
    }
}