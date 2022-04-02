using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Items
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
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailureCallback);
                notificationService.NotifyError("Page loading failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailureCallback);
                notificationService.NotifyError("Unknown error while loading page", e.Message, fragment);
            }
            return null;
        }

        public async Task<IEnumerable<SearchItemResult>> SearchItemsAsync(string searchInput,
            IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.SearchItemsAsync(searchInput);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(searchInput, fragmentCreator));
                notificationService.NotifyError("Items loading failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(searchInput, fragmentCreator));
                notificationService.NotifyError("Unknown error while loading items", e.Message, fragment);
            }

            return null;
        }

        public async Task<IEnumerable<SearchItemResult>> SearchItemsAsync(IEnumerable<Guid> storeIds, IEnumerable<Guid> itemCategoryIds,
            IEnumerable<Guid> manufacturerIds, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.SearchItemsByFilterAsync(
                        storeIds,
                        itemCategoryIds,
                        manufacturerIds);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(storeIds, itemCategoryIds, manufacturerIds, fragmentCreator));
                notificationService.NotifyError("Items loading failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(storeIds, itemCategoryIds, manufacturerIds, fragmentCreator));
                notificationService.NotifyError("Unknown error while loading items", e.Message, fragment);
            }
            return null;
        }

        public async Task<IEnumerable<Manufacturer>> LoadManufacturersAsync(IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetAllActiveManufacturersAsync();
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadManufacturersAsync(fragmentCreator));
                notificationService.NotifyError("Loading manufacturers failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadManufacturersAsync(fragmentCreator));
                notificationService.NotifyError("Unknown error while loading manufacturers", e.Message, fragment);
            }
            return null;
        }

        public async Task<IEnumerable<ItemCategory>> LoadItemCategoriesAsync(IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetAllActiveItemCategoriesAsync();
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadItemCategoriesAsync(fragmentCreator));
                notificationService.NotifyError("Loading item categories failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadItemCategoriesAsync(fragmentCreator));
                notificationService.NotifyError("Unknown error while loading item categories", e.Message, fragment);
            }
            return null;
        }

        public async Task<StoreItem> LoadItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetItemByIdAsync(itemId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadItemAsync(itemId, fragmentCreator));
                notificationService.NotifyError("Loading item failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await LoadItemAsync(itemId, fragmentCreator));
                notificationService.NotifyError("Unknown error while loading item", e.Message, fragment);
            }
            return null;
        }
    }
}