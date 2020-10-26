using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Ports
{
    public interface IShoppingListRepository
    {
        Task<bool> ActiveShoppingListExistsForAsync(StoreId storeId, CancellationToken cancellationToken);

        Task<Models.ShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ShoppingListNotFoundException"></exception>
        Task<Models.ShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ItemNotOnShoppingListException"></exception>
        Task StoreAsync(Models.ShoppingList shoppingList, CancellationToken cancellationToken);
    }
}