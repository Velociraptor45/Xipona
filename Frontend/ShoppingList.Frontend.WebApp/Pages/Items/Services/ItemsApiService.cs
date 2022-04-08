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

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services
{
    public class ItemsApiService : IItemsApiService
    {
        private readonly IApiClient apiClient;
        private readonly IShoppingListNotificationService notificationService;

        public ItemsApiService(IApiClient apiClient, IShoppingListNotificationService notificationService)
        {
            this.apiClient = apiClient;
            this.notificationService = notificationService;
        }

        public async Task LoadInitialPageStateAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Action<ItemsState> onSuccessAction)
        {
            try
            {
                var stores = await apiClient.GetAllActiveStoresAsync();
                var manufacturers = await apiClient.GetAllActiveManufacturersAsync();
                var itemCategories = await apiClient.GetAllActiveItemCategoriesAsync();
                var quantityTypes = await apiClient.GetAllQuantityTypesAsync();
                var quantityTypesInPacket = await apiClient.GetAllQuantityTypesInPacketAsync();

                var state = new ItemsState(
                    stores,
                    itemCategories,
                    manufacturers,
                    quantityTypes,
                    quantityTypesInPacket);

                onSuccessAction(state);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadInitialPageStateAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Page loading failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadInitialPageStateAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading page", e.Message, fragment);
            }
        }

        public async Task SearchItemsAsync(string searchInput, IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<SearchItemResult>> onSuccessAction)
        {
            try
            {
                var results = await apiClient.SearchItemsAsync(searchInput);
                onSuccessAction(results);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(searchInput, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Items loading failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(searchInput, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading items", e.Message, fragment);
            }
        }

        public async Task SearchItemsAsync(IEnumerable<Guid> storeIds, IEnumerable<Guid> itemCategoryIds,
            IEnumerable<Guid> manufacturerIds, IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<SearchItemResult>> onSuccessAction)
        {
            try
            {
                var results = await apiClient.SearchItemsByFilterAsync(
                        storeIds,
                        itemCategoryIds,
                        manufacturerIds);

                onSuccessAction(results);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(storeIds, itemCategoryIds, manufacturerIds, fragmentCreator,
                        onSuccessAction));
                notificationService.NotifyError("Items loading failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SearchItemsAsync(storeIds, itemCategoryIds, manufacturerIds, fragmentCreator,
                        onSuccessAction));
                notificationService.NotifyError("Unknown error while loading items", e.Message, fragment);
            }
        }

        public async Task LoadManufacturersAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<Manufacturer>> onSuccessAction)
        {
            try
            {
                var results = await apiClient.GetAllActiveManufacturersAsync();
                onSuccessAction(results);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadManufacturersAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Loading manufacturers failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadManufacturersAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading manufacturers", e.Message, fragment);
            }
        }

        public async Task LoadItemCategoriesAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<ItemCategory>> onSuccessAction)
        {
            try
            {
                var results = await apiClient.GetAllActiveItemCategoriesAsync();
                onSuccessAction(results);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemCategoriesAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Loading item categories failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemCategoriesAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading item categories", e.Message, fragment);
            }
        }

        public async Task LoadItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator,
            Action<StoreItem> onSuccessAction)
        {
            try
            {
                var result = await apiClient.GetItemByIdAsync(itemId);
                onSuccessAction(result);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemAsync(itemId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Loading item failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemAsync(itemId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading item", e.Message, fragment);
            }
        }
    }
}