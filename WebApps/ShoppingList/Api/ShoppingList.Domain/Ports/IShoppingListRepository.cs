using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IShoppingListRepository
    {
        Task<bool> ActiveShoppingListExistsForAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<Models.ShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<Store>> FindActiveStoresAsync(CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ShoppingListNotFoundException"></exception>
        Task<Models.ShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ItemNotOnShoppingListException"></exception>
        Task StoreAsync(Models.ShoppingList shoppingList, CancellationToken cancellationToken);
    }
}