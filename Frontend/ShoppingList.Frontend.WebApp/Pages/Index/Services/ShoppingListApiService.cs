using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public class ShoppingListApiService : IShoppingListApiService
    {
        private readonly IApiClient apiClient;
        private readonly ICommandQueue commandQueue;
        private readonly IShoppingListNotificationService notificationService;

        public ShoppingListApiService(IApiClient apiClient, ICommandQueue commandQueue,
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

        public async Task<ShoppingListRoot> LoadActiveShoppingListAsync(Guid storeId,
            IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetActiveShoppingListByStoreIdAsync(storeId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadActiveShoppingListAsync(storeId, fragmentCreator));
                notificationService.NotifyError("Loading shopping list failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadActiveShoppingListAsync(storeId, fragmentCreator));
                notificationService.NotifyError("Unknown error while loading shopping list", e.Message, fragment);
            }

            return null;
        }

        public async Task FinishListAsync(Guid shoppingListId, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction)
        {
            var request = new FinishListRequest(Guid.NewGuid(), shoppingListId);

            try
            {
                await apiClient.FinishListAsync(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await FinishListAsync(shoppingListId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Finishing shopping list failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await FinishListAsync(shoppingListId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while finishing shopping list", e.Message, fragment);
            }

            await onSuccessAction();
        }

        public async Task<IEnumerable<SearchItemForShoppingListResult>> LoadItemSearchResultAsync(string input,
            Guid storeId, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.SearchItemsForShoppingListAsync(input, storeId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemSearchResultAsync(input, storeId, fragmentCreator));
                notificationService.NotifyError("Searching for items failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemSearchResultAsync(input, storeId, fragmentCreator));
                notificationService.NotifyError("Unknown error while searching for items", e.Message, fragment);
            }

            return null;
        }

        public async Task AddItemToShoppingListAsync(Guid shoppingListId, ItemId itemId, int quantity,
            Guid sectionId, IAsyncRetryFragmentCreator fragmentCreator, Func<Task> onSuccessAction)
        {
            var request = new AddItemToShoppingListRequest(
                Guid.NewGuid(),
                shoppingListId,
                itemId,
                quantity,
                sectionId);

            try
            {
                await apiClient.AddItemToShoppingListAsync(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await AddItemToShoppingListAsync(shoppingListId, itemId, quantity, sectionId, fragmentCreator,
                        onSuccessAction));
                notificationService.NotifyError("Adding item failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await AddItemToShoppingListAsync(shoppingListId, itemId, quantity, sectionId, fragmentCreator,
                        onSuccessAction));
                notificationService.NotifyError("Unknown error while adding item", e.Message, fragment);
            }

            await onSuccessAction();
        }

        public async Task AddItemWithTypeToShoppingListAsync(Guid shoppingListId, Guid itemId, Guid itemTypeId,
            int quantity, Guid sectionId, IAsyncRetryFragmentCreator fragmentCreator, Func<Task> onSuccessAction)
        {
            var request = new AddItemWithTypeToShoppingListRequest(
                Guid.NewGuid(),
                shoppingListId,
                itemId,
                itemTypeId,
                quantity,
                sectionId);

            try
            {
                await apiClient.AddItemWithTypeToShoppingListAsync(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await AddItemWithTypeToShoppingListAsync(shoppingListId, itemId, itemTypeId, quantity, sectionId,
                        fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Adding item failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await AddItemWithTypeToShoppingListAsync(shoppingListId, itemId, itemTypeId, quantity, sectionId,
                        fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while adding item", e.Message, fragment);
            }

            await onSuccessAction();
        }

        public async Task<IEnumerable<Store>> LoadAllActiveStoresAsync(IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await apiClient.GetAllActiveStoresAsync();
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(
                        async () => await LoadAllActiveStoresAsync(fragmentCreator));
                notificationService.NotifyError("Loading active stores failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(
                    async () => await LoadAllActiveStoresAsync(fragmentCreator));
                notificationService.NotifyError("Unknown error while loading active stores", e.Message, fragment);
            }

            return null;
        }
    }
}