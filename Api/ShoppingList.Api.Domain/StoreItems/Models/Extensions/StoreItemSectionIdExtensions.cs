using ProjectHermes.ShoppingList.Api.Domain.Sections.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemSectionIdExtensions
    {
        public static SectionId ToShoppingListSectionId(this StoreItemSectionId id)
        {
            return new SectionId(id.Value);
        }
    }
}