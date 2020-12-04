using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public class ItemCategoryId : GenericPrimitive<int>
    {
        public ItemCategoryId(int id)
            : base(id)
        {
        }
    }
}