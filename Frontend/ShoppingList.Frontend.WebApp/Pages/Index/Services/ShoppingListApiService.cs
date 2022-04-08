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

        public async Task ChangeItemQuantityOnShoppingListAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId,
            float quantity)
        {
            var request = new ChangeItemQuantityOnShoppingListRequest(Guid.NewGuid(), shoppingListId, itemId,
                itemTypeId, quantity);
            await EnqueueAsync(request);
        }

        public async Task RemoveItemFromShoppingListAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId)
        {
            var request = new RemoveItemFromShoppingListRequest(Guid.NewGuid(), shoppingListId, itemId, itemTypeId);
            await EnqueueAsync(request);
        }

        public async Task RemoveItemFromBasketAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId)
        {
            var request = new RemoveItemFromBasketRequest(Guid.NewGuid(), shoppingListId, itemId, itemTypeId);
            await EnqueueAsync(request);
        }

        public async Task PutItemInBasketAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId)
        {
            var request = new PutItemInBasketRequest(Guid.NewGuid(), shoppingListId, itemId, itemTypeId);
            await EnqueueAsync(request);
        }

        public async Task CreateTemporaryItemOnShoppingListAsync(ShoppingListItem item, Guid shoppingListId,
            Guid storeId, StoreSectionId sectionId)
        {
            var createRequest = new CreateTemporaryItemRequest(Guid.NewGuid(), item.Id.OfflineId!.Value, item.Name,
                storeId, item.PricePerQuantity, sectionId.BackendId);
            var addRequest =
                new AddItemToShoppingListRequest(Guid.NewGuid(), shoppingListId, item.Id, item.Quantity, null);

            await EnqueueAsync(createRequest);
            await EnqueueAsync(addRequest);
        }

        public async Task LoadActiveShoppingListAsync(Guid storeId,
            IAsyncRetryFragmentCreator fragmentCreator, Action<ShoppingListRoot> onSuccessAction)
        {
            ShoppingListRoot shoppingList;

            try
            {
                shoppingList = await apiClient.GetActiveShoppingListByStoreIdAsync(storeId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadActiveShoppingListAsync(storeId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Loading shopping list failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadActiveShoppingListAsync(storeId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading shopping list", e.Message, fragment);
                return;
            }

            onSuccessAction(shoppingList);
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

        public async Task LoadItemSearchResultAsync(string input, Guid storeId,
            IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<SearchItemForShoppingListResult>> onSuccessAction)
        {
            IEnumerable<SearchItemForShoppingListResult> result;

            try
            {
                result = await apiClient.SearchItemsForShoppingListAsync(input, storeId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemSearchResultAsync(input, storeId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Searching for items failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadItemSearchResultAsync(input, storeId, fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while searching for items", e.Message, fragment);
                return;
            }

            onSuccessAction(result);
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

        public async Task LoadAllActiveStoresAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Func<IEnumerable<Store>, Task> onSuccessAction)
        {
            IEnumerable<Store> result;

            try
            {
                result = await apiClient.GetAllActiveStoresAsync();
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(
                        async () => await LoadAllActiveStoresAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Loading active stores failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(
                    async () => await LoadAllActiveStoresAsync(fragmentCreator, onSuccessAction));
                notificationService.NotifyError("Unknown error while loading active stores", e.Message, fragment);
                return;
            }

            await onSuccessAction(result);
        }

        private async Task EnqueueAsync(IApiRequest request)
        {
            await commandQueue.Enqueue(request);
        }
    }
}