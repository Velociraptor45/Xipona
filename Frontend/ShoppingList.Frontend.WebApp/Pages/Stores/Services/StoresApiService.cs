using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
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
        private readonly IApiClient apiClient;
        private readonly IShoppingListNotificationService notificationService;

        public StoresApiService(IApiClient apiClient, IShoppingListNotificationService notificationService)
        {
            this.apiClient = apiClient;
            this.notificationService = notificationService;
        }

        public async Task SaveStoreAsync(Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction)
        {
            var request = new ModifyStoreRequest(Guid.NewGuid(), store.Id, store.Name, store.Sections);

            try
            {
                await apiClient.ModifyStoreAsync(request);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await SaveStoreAsync(store, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Saving store failed", e.Message, fragment);
            }

            await onSuccessAction();
        }

        public async Task CreateStoreAsync(Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction)
        {
            var request = new CreateStoreRequest(Guid.NewGuid(), store);

            try
            {
                await apiClient.CreateStoreAsync(request);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateStoreAsync(store, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Creating store failed", e.Message, fragment);
            }

            await onSuccessAction();
        }

        public async Task LoadStores(IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<Store>> onSuccessAction)
        {
            IEnumerable<Store> stores;

            try
            {
                stores = await apiClient.GetAllActiveStoresAsync();
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment =
                    fragmentCreator.CreateAsyncRetryFragment(async () =>
                        await LoadStores(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Loading stores failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment =
                    fragmentCreator.CreateAsyncRetryFragment(async () =>
                        await LoadStores(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading stores", e.Message, fragment);
                return;
            }

            onSuccessAction(stores);
        }
    }
}