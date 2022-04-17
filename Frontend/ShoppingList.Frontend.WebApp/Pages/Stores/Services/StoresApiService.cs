using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Stores;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services
{
    public class StoresApiService : IStoresApiService
    {
        private readonly IApiClient _apiClient;
        private readonly IShoppingListNotificationService _notificationService;

        public StoresApiService(IApiClient apiClient, IShoppingListNotificationService notificationService)
        {
            _apiClient = apiClient;
            _notificationService = notificationService;
        }

        public async Task SaveStoreAsync(Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction)
        {
            var request = new ModifyStoreRequest(Guid.NewGuid(), store.Id, store.Name, store.Sections);

            try
            {
                await _apiClient.ModifyStoreAsync(request);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SaveStoreAsync(store, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Saving store failed", e.Message, fragment);
            }

            await onSuccessAction();
        }

        public async Task CreateStoreAsync(Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction)
        {
            var request = new CreateStoreRequest(Guid.NewGuid(), store);

            try
            {
                await _apiClient.CreateStoreAsync(request);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateStoreAsync(store, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Creating store failed", e.Message, fragment);
            }

            await onSuccessAction();
        }

        public async Task LoadStores(IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<Store>> onSuccessAction)
        {
            IEnumerable<Store> stores;

            try
            {
                stores = await _apiClient.GetAllActiveStoresAsync();
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment =
                    fragmentCreator.CreateAsyncRetryFragment(async () =>
                        await LoadStores(fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Loading stores failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment =
                    fragmentCreator.CreateAsyncRetryFragment(async () =>
                        await LoadStores(fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while loading stores", e.Message, fragment);
                return;
            }

            onSuccessAction(stores);
        }
    }
}