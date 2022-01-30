using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Models
{
    public class ItemCategoryBuilder : DomainTestBuilderBase<ItemCategory>
    {
        public ItemCategoryBuilder WithId(ItemCategoryId id)
        {
            FillConstructorWith("id", id);
            return this;
        }

        public ItemCategoryBuilder WithIsDeleted(bool isDeleted)
        {
            FillConstructorWith("isDeleted", isDeleted);
            return this;
        }
    }
}