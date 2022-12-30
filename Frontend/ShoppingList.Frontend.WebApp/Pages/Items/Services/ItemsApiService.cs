using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using RestEase;
using ShoppingList.Frontend.Redux.Shared.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services
{
    public class ItemsApiService : IItemsApiService
    {
        private readonly IApiClient _apiClient;
        private readonly IShoppingListNotificationService _notificationService;

        public ItemsApiService(IApiClient apiClient, IShoppingListNotificationService notificationService)
        {
            _apiClient = apiClient;
            _notificationService = notificationService;
        }

        public async Task LoadInitialPageStateAsync(ItemsState state, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                var stores = await _apiClient.GetAllActiveStoresAsync();
                var manufacturers = await _apiClient.GetAllActiveManufacturersAsync();
                var itemCategories = await _apiClient.GetAllActiveItemCategoriesAsync();
                //var quantityTypes = await _apiClient.GetAllQuantityTypesAsync();
                //var quantityTypesInPacket = await _apiClient.GetAllQuantityTypesInPacketAsync();

                state.Initialize(
                    stores,
                    itemCategories,
                    manufacturers,
                    new List<QuantityType>(),
                    new List<QuantityTypeInPacket>());
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadInitialPageStateAsync(state, fragmentCreator));
                _notificationService.NotifyError("Page loading failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadInitialPageStateAsync(state, fragmentCreator));
                _notificationService.NotifyError("Unknown error while loading page", e.Message, fragment);
            }
        }

        public async Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemsByItemCategoryAsync(
            Guid itemCategoryId)
        {
            try
            {
                return await _apiClient.SearchItemByItemCategoryAsync(itemCategoryId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();
                _notificationService.NotifyError("Searching for items by item category failed", contract.Message);
            }
            catch (Exception e)
            {
                _notificationService.NotifyError("Unknown error while searching for items by item category", e.Message);
            }

            return Enumerable.Empty<SearchItemByItemCategoryResult>();
        }

        public async Task LoadItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator,
            Action<Item> onSuccessAction)
        {
            try
            {
                var result = await _apiClient.GetItemByIdAsync(itemId);
                onSuccessAction(result);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemAsync(itemId, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Loading item failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemAsync(itemId, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while loading item", e.Message, fragment);
            }
        }
    }
}