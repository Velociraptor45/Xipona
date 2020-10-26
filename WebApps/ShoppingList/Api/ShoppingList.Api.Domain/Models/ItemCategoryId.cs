using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class ItemCategoryId : GenericPrimitive<int>
    {
        public ItemCategoryId(int id)
            : base(id)
        {
        }
    }
}