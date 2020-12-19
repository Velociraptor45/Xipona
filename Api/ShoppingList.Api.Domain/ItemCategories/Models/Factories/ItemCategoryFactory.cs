using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories
{
    public class ItemCategoryFactory : IItemCategoryFactory
    {
        public IItemCategory Create(ItemCategoryId id, string name, bool isDeleted)
        {
            return new ItemCategory(id, name, isDeleted);
        }
    }
}