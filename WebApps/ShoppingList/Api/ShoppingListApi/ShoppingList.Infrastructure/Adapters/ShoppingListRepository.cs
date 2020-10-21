using Microsoft.EntityFrameworkCore;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Ports;
using ShoppingList.Infrastructure.Converters;
using ShoppingList.Infrastructure.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

using Models = ShoppingList.Domain.Models;

namespace ShoppingList.Infrastructure.Adapters
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingContext dbContext;

        public ShoppingListRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Models.ShoppingList> FindActiveByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var list = await dbContext.ShoppingLists.AsNoTracking()
                    .FirstOrDefaultAsync(list => list.CompletionDate != null
                        && list.StoreId == storeId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return list?.ToDomain();
        }
    }
}