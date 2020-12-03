using ShoppingList.Api.Core;
using System;

namespace ShoppingList.Api.Domain.Models
{
    public class ShoppingListItemOfflineId : GenericPrimitive<Guid>
    {
        public ShoppingListItemOfflineId(Guid id)
            : base(id)
        {
        }
    }
}