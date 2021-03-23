using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class ItemId : GenericPrimitive<int>
    {
        public ItemId(int id) : base(id)
        {
        }
    }
}