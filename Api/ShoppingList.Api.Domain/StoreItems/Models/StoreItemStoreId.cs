using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemStoreId : GenericPrimitive<int>, IEqualityComparer<StoreId>
    {
        public StoreItemStoreId(int value) : base(value)
        {
        }

        public StoreId ToShoppingListStoreId()
        {
            return new StoreId(Value);
        }

        public bool Equals([AllowNull] StoreId x, [AllowNull] StoreId y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;

            return x.Value == y.Value;
        }

        public int GetHashCode([DisallowNull] StoreId obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}