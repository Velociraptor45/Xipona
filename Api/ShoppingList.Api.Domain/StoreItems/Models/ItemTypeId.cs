using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class ItemTypeId : GenericPrimitive<int>
    {
        public ItemTypeId(int value) : base(value)
        {
        }
    }
}