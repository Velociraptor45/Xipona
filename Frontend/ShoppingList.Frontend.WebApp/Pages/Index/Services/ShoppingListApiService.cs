using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Error;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using RestEase;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public class ShoppingListApiService : IShoppingListApiService
    {
        private readonly IApiClient _apiClient;
        private readonly ICommandQueue _commandQueue;
        private readonly IShoppingListNotificationService _notificationService;

        public ShoppingListApiService(IApiClient apiClient, ICommandQueue commandQueue,
            IShoppingListNotificationService notificationService)
        {
            _apiClient = apiClient;
            _commandQueue = commandQueue;
            _notificationService = notificationService;
        }

        public void InitializeCommandQueue(ICommandQueueErrorHandler errorHandler)
        {
            //_commandQueue.Initialize(errorHandler); //todo
        }

        public async Task LoadActiveShoppingListAsync(Guid storeId,
            IAsyncRetryFragmentCreator fragmentCreator, Action<ShoppingListRoot> onSuccessAction)
        {
            ShoppingListRoot shoppingList;

            try
            {
                shoppingList = await _apiClient.GetActiveShoppingListByStoreIdAsync(storeId);
            }
            catch (ApiException e)
            {
                ErrorContract contract = null;
                if (e.Message.Contains("errorCode"))
                    contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadActiveShoppingListAsync(storeId, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Loading shopping list failed", contract?.Message ?? e.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await LoadActiveShoppingListAsync(storeId, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while loading shopping list", e.Message, fragment);
                return;
            }

            onSuccessAction(shoppingList);
        }

        public async Task FinishListAsync(Guid shoppingListId, DateTimeOffset? finishedAt, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction)
        {
            var request = new FinishListRequest(Guid.NewGuid(), shoppingListId, finishedAt);

            try
            {
                await _apiClient.FinishListAsync(request);
            }
            catch (ApiException e)
            {
                ErrorContract contract = null;
                if (e.Message.Contains("errorCode"))
                    contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await FinishListAsync(shoppingListId, finishedAt, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Finishing shopping list failed", contract?.Message ?? e.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await FinishListAsync(shoppingListId, finishedAt, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while finishing shopping list", e.Message, fragment);
                return;
            }

            await onSuccessAction();
        }

        private async Task EnqueueAsync(IApiRequest request)
        {
            await _commandQueue.Enqueue(request);
        }
    }
}