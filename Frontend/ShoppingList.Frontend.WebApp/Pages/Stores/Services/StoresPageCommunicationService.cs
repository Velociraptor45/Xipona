using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Common.Error;
using ProjectHermes.ShoppingList.Frontend.Models.Common.Services;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services
{
    public class StoresPageCommunicationService : IStoresPageCommunicationService
    {
        private readonly IApiClient apiClient;
        private readonly IShoppingListNotificationService notificationService;

        public StoresPageCommunicationService(IApiClient apiClient, IShoppingListNotificationService notificationService)
        {
            this.apiClient = apiClient;
            this.notificationService = notificationService;
        }

        public async Task<bool> SaveStoreAsync(ModifyStoreRequest request, Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                await apiClient.ModifyStoreAsync(request);
                return true;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await OnFailure());
                notificationService.NotifyError("Saving store failed", e.Message, fragment);
            }

            return false;
        }

        public async Task<bool> CreateStoreAsync(CreateStoreRequest request, Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                await apiClient.CreateStoreAsync(request);
                return true;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await OnFailure());
                notificationService.NotifyError("Creating store failed", e.Message, fragment);
            }
            return false;
        }

        public async Task<IEnumerable<Store>> LoadStores(Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetAllActiveStoresAsync();
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () => await OnFailure());
                notificationService.NotifyError("Loading stores failed", e.Message, fragment);
            }
            return null;
        }
    }
}