using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public class StoreId : GenericPrimitive<int>
    {
        public StoreId(int id)
            : base(id)
        {
        }
    }
}