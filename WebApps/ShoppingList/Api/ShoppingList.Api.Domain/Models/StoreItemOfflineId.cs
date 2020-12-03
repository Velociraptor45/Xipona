using ShoppingList.Api.Core;
using System;

namespace ShoppingList.Api.Domain.Models
{
    public class StoreItemOfflineId : GenericPrimitive<Guid>
    {
        public StoreItemOfflineId(Guid id)
            : base(id)
        {
        }
    }
}