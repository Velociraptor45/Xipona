using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemActualId : GenericPrimitive<int>
    {
        public StoreItemActualId(int id)
            : base(id)
        {
        }
    }
}