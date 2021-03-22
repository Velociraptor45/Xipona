using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemStoreId : GenericPrimitive<int>, IEqualityComparer<ShoppingLists.Models.ShoppingListStoreId>
    {
        public StoreItemStoreId(int value) : base(value)
        {
        }

        public ShoppingLists.Models.ShoppingListStoreId ToShoppingListStoreId()
        {
            return new ShoppingLists.Models.ShoppingListStoreId(Value);
        }

        public Stores.Models.StoreId AsStoreId()
        {
            return new Stores.Models.StoreId(Value);
        }

        public bool Equals([AllowNull] ShoppingLists.Models.ShoppingListStoreId x, [AllowNull] ShoppingLists.Models.ShoppingListStoreId y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;

            return x.Value == y.Value;
        }

        public int GetHashCode([DisallowNull] ShoppingLists.Models.ShoppingListStoreId obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}