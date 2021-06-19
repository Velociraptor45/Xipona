using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services
{
    public class ItemsPageLoadingService : IItemsPageLoadingService
    {
        private readonly IApiClient apiClient;
        private readonly IShoppingListNotificationService notificationService;

        public ItemsPageLoadingService(IApiClient apiClient, IShoppingListNotificationService notificationService)
        {
            this.apiClient = apiClient;
            this.notificationService = notificationService;
        }

        public async Task<ItemsState> LoadInitialPageState(Func<Task> OnFailureCallback, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                var stores = await apiClient.GetAllActiveStoresAsync();
                var manufacturers = await apiClient.GetAllActiveManufacturersAsync();
                var itemCategories = await apiClient.GetAllActiveItemCategoriesAsync();
                var quantityTypes = await apiClient.GetAllQuantityTypesAsync();
                var quantityTypesInPacket = await apiClient.GetAllQuantityTypesInPacketAsync();

                return new ItemsState(
                    stores,
                    itemCategories,
                    manufacturers,
                    quantityTypes,
                    quantityTypesInPacket);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailureCallback);
                notificationService.NotifyError("Page loading failed", e.Message, fragment);
            }
            return null;
        }

        public async Task<IEnumerable<ItemFilterResult>> LoadItemsAsync(IEnumerable<int> storeIds, IEnumerable<int> itemCategoryIds,
            IEnumerable<int> manufacturerIds, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetItemFilterResultAsync(
                        storeIds,
                        itemCategoryIds,
                        manufacturerIds);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemsAsync(storeIds, itemCategoryIds, manufacturerIds, fragmentCreator));
                notificationService.NotifyError("Item loading failed", e.Message, fragment);
            }
            return null;
        }

        public async Task<IEnumerable<Manufacturer>> LoadManufacturersAsync(IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetAllActiveManufacturersAsync();
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadManufacturersAsync(fragmentCreator));
                notificationService.NotifyError("Loading manufacturers failed", e.Message, fragment);
            }
            return null;
        }

        public async Task<IEnumerable<ItemCategory>> LoadItemCategoriesAsync(IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetAllActiveItemCategoriesAsync();
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadItemCategoriesAsync(fragmentCreator));
                notificationService.NotifyError("Loading manufacturers failed", e.Message, fragment);
            }
            return null;
        }

        public async Task<StoreItem> LoadItemAsync(int itemId, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetItemByIdAsync(itemId);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadItemAsync(itemId, fragmentCreator));
                notificationService.NotifyError("Loading item failed", e.Message, fragment);
            }
            return null;
        }
    }
}