using ShoppingList.Api.Client;
using ShoppingList.Api.Contracts.Queries;
using ShoppingList.Api.Contracts.Queries.AllActiveStores;
using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Frontend.Infrastructure.Exceptions;
using ShoppingList.Frontend.Infrastructure.Extensions.Contracts;
using ShoppingList.Frontend.Models;
using ShoppingList.Frontend.Models.Index.Search;
using ShoppingList.Frontend.Models.Items;
using ShoppingList.Frontend.Models.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public class OfflineClient : IOfflineClient
    {
        private readonly IShoppingListApiClient client;
        private readonly int retryAttempts;
        private readonly int retryTimoutInMilliseconds;

        public OfflineClient(IShoppingListApiClient client)
        {
            this.client = client;
            retryAttempts = 20;
            retryTimoutInMilliseconds = 5000;
        }

        public async Task AddItemToShoppingListAsync(int shoppingListId, int itemId, float quantity)
        {
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    await client.AddItemToShoppingList(shoppingListId, itemId, quantity);
                    return;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(int shoppingListId, int itemId, float quantity)
        {
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    await client.ChangeItemQuantityOnShoppingList(shoppingListId, itemId, quantity);
                    return;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }
        }

        public async Task FinishListAsync(int shoppingListId)
        {
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    await client.FinishList(shoppingListId);
                    return;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }
        }

        public async Task<ShoppingListRoot> GetActiveShoppingListByStoreIdAsync(int storeId)
        {
            ShoppingListContract list = null;
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    list = await client.GetActiveShoppingListByStoreId(storeId);
                    break;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }

            if (list == null)
                throw new RetryException();

            return list.ToModel();
        }

        public async Task<IEnumerable<Store>> GetAllActiveStoresAsync()
        {
            IEnumerable<ActiveStoreContract> stores = null;
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    stores = await client.GetAllActiveStores();
                    break;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }

            if (stores == null)
                throw new RetryException();

            return stores.Select(store => store.ToModel());
        }

        public async Task<IEnumerable<Manufacturer>> GetAllActiveManufacturers()
        {
            var manufacturers = await client.GetAllActiveManufacturers();

            return manufacturers.Select(man => man.ToModel());
        }

        public async Task<IEnumerable<ItemCategory>> GetAllActiveItemCategories()
        {
            var itemCategories = await client.GetAllActiveItemCategories();

            return itemCategories.Select(cat => cat.ToModel());
        }

        public async Task<IEnumerable<ItemSearchResult>> GetItemSearchResultsAsync(string searchInput, int storeId)
        {
            IEnumerable<ItemSearchContract> results = null;
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    results = await client.GetItemSearchResults(searchInput, storeId);
                    break;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }

            if (results == null)
                throw new RetryException();

            return results.Select(result => result.ToModel());
        }

        public async Task PutItemInBasketAsync(int shoppingListId, int itemId)
        {
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    await client.PutItemInBasket(shoppingListId, itemId);
                    return;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }
        }

        public async Task RemoveItemFromBasketAsync(int shoppingListId, int itemId)
        {
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    await client.RemoveItemFromBasket(shoppingListId, itemId);
                    return;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }
        }

        public async Task RemoveItemFromShoppingListAsync(int shoppingListId, int itemId)
        {
            for (int i = 0; i < retryAttempts; i++)
            {
                try
                {
                    await client.RemoveItemFromShoppingList(shoppingListId, itemId);
                    return;
                }
                catch (JSException e)
                {
                    await Task.Delay(retryTimoutInMilliseconds);
                }
            }
        }

        public async Task<IEnumerable<ItemFilterResult>> GetItemFilterResult(IEnumerable<int> storeIds,
            IEnumerable<int> itemCategoryIds, IEnumerable<int> manufacturerIds)
        {
            var result = await client.GetItemFilterResult(
                storeIds,
                itemCategoryIds,
                manufacturerIds);

            return result.Select(r => r.ToModel());
        }

        public async Task<StoreItem> GetItemById(int itemId)
        {
            var result = await client.Get(itemId);
            return result.ToModel();
        }
    }
}