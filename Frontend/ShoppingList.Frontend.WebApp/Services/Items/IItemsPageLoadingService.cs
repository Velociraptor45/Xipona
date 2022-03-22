using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Items
{
    public interface IItemsPageLoadingService
    {
        Task<ItemsState> LoadInitialPageState(Func<Task> OnFailureCallback, IAsyncRetryFragmentCreator fragmentCreator);
        Task<StoreItem> LoadItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<ItemCategory>> LoadItemCategoriesAsync(IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<SearchItemResult>> SearchItemsAsync(IEnumerable<Guid> storeIds, IEnumerable<Guid> itemCategoryIds,
            IEnumerable<Guid> manufacturerIds, IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<Manufacturer>> LoadManufacturersAsync(IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<SearchItemResult>> SearchItemsAsync(string searchInput,
            IAsyncRetryFragmentCreator fragmentCreator);
    }
}