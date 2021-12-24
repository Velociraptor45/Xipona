using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public class ShoppingListCommunicationService : IShoppingListCommunicationService
    {
        private readonly IApiClient apiClient;
        private readonly ICommandQueue commandQueue;
        private readonly IShoppingListNotificationService notificationService;

        public ShoppingListCommunicationService(IApiClient apiClient, ICommandQueue commandQueue,
            IShoppingListNotificationService notificationService)
        {
            this.apiClient = apiClient;
            this.commandQueue = commandQueue;
            this.notificationService = notificationService;
        }

        public void InitializeCommandQueue(ICommandQueueErrorHandler errorHandler)
        {
            commandQueue.Initialize(errorHandler);
        }

        public async Task EnqueueAsync(IApiRequest request)
        {
            await commandQueue.Enqueue(request);
        }

        public async Task<ShoppingListRoot> LoadActiveShoppingListAsync(int storeId, Func<Task> OnFailure,
            IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetActiveShoppingListByStoreIdAsync(storeId);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailure);
                notificationService.NotifyError("Loading shopping list failed", e.Message, fragment);
            }

            return null;
        }

        public async Task<bool> FinishListAsync(FinishListRequest request, Func<Task> OnFailure, 
            IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                await apiClient.FinishListAsync(request);
                return true;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailure);
                notificationService.NotifyError("Finishing shopping list failed", e.Message, fragment);
            }

            return false;
        }

        public async Task<IEnumerable<ItemSearchResult>> LoadItemSearchResultAsync(string input, int storeId, 
            Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetItemSearchResultsAsync(input, storeId);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailure);
                notificationService.NotifyError("Search for items failed", e.Message, fragment);
            }

            return null;
        }

        public async Task<bool> AddItemToShoppingListAsync(AddItemToShoppingListRequest request,
            Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                await apiClient.AddItemToShoppingListAsync(request);
                return true;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailure);
                notificationService.NotifyError("Adding item failed", e.Message, fragment);
            }

            return false;
        }

        public async Task<bool> AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request,
            Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                await apiClient.AddItemWithTypeToShoppingListAsync(request);
                return true;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailure);
                notificationService.NotifyError("Adding item failed", e.Message, fragment);
            }

            return false;
        }

        public async Task<IEnumerable<Store>> LoadAllActiveStoresAsync(Func<Task> OnFailure,
            IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetAllActiveStoresAsync();
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(OnFailure);
                notificationService.NotifyError("Page loading failed", e.Message, fragment);
            }

            return null;
        }
    }
}
