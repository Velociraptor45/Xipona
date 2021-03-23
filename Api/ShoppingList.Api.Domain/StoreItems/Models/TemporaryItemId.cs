using ProjectHermes.ShoppingList.Api.Core;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class TemporaryItemId : GenericPrimitive<Guid>
    {
        public TemporaryItemId(Guid id) : base(id)
        {
        }
    }
}