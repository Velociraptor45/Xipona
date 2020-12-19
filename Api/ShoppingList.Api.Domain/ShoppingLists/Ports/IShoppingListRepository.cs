using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports
{
    public interface IShoppingListRepository
    {
        Task<bool> ActiveShoppingListExistsForAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<IEnumerable<IShoppingList>> FindActiveByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ShoppingListNotFoundException"></exception>
        Task<IShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken);

        Task<IEnumerable<IShoppingList>> FindByAsync(StoreItemId storeItemId, CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ItemNotOnShoppingListException"></exception>
        Task StoreAsync(IShoppingList shoppingList, CancellationToken cancellationToken);
    }
}