using ProjectHermes.ShoppingList.Api.Core;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemOfflineId : GenericPrimitive<Guid>
    {
        public StoreItemOfflineId(Guid id)
            : base(id)
        {
        }
    }
}