using ShoppingList.Core;

namespace ShoppingList.Domain.Models
{
    public class ItemCategoryId : GenericPrimitive<int>
    {
        public ItemCategoryId(int id)
            : base(id)
        {
        }
    }
}