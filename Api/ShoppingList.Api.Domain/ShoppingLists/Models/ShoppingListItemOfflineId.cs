using ProjectHermes.ShoppingList.Api.Core;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListItemOfflineId : GenericPrimitive<Guid>
    {
        public ShoppingListItemOfflineId(Guid id)
            : base(id)
        {
        }
    }
}