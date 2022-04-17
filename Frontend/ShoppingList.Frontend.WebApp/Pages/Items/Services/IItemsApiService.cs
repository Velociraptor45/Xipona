using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services
{
    public interface IItemsApiService
    {
        Task LoadInitialPageStateAsync(IAsyncRetryFragmentCreator fragmentCreator, Action<ItemsState> onSuccessAction);

        Task LoadItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator, Action<Item> onSuccessAction);

        Task LoadItemCategoriesAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<ItemCategory>> onSuccessAction);

        Task SearchItemsAsync(IEnumerable<Guid> storeIds, IEnumerable<Guid> itemCategoryIds,
            IEnumerable<Guid> manufacturerIds, IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<SearchItemResult>> onSuccessAction);

        Task SearchItemsAsync(string searchInput, IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<SearchItemResult>> onSuccessAction);

        Task LoadManufacturersAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<Manufacturer>> onSuccessAction);
    }
}