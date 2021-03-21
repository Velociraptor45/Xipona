using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models
{
    public class ItemCategoryId : GenericPrimitive<int>
    {
        public ItemCategoryId(int id)
            : base(id)
        {
        }
    }
}