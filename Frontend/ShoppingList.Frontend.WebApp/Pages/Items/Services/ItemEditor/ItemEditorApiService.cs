using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using RestEase;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services.ItemEditor
{
    public class ItemEditorApiService : IItemEditorApiService
    {
        private readonly IShoppingListNotificationService _notificationService;
        private readonly IApiClient _apiClient;

        public ItemEditorApiService(IShoppingListNotificationService notificationService, IApiClient apiClient)
        {
            _notificationService = notificationService;
            _apiClient = apiClient;
        }

        public async Task UpdateItemWithTypesAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var withTypesRequest = new UpdateItemWithTypesRequest(Guid.NewGuid(), item);
            try
            {
                await _apiClient.UpdateItemWithTypesAsync(withTypesRequest);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await UpdateItemWithTypesAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Updating item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await UpdateItemWithTypesAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while updating item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task UpdateItemAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var request = new UpdateItemRequest(Guid.NewGuid(), item);
            try
            {
                await _apiClient.UpdateItemAsync(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await UpdateItemAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Updating item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await UpdateItemAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while updating item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task CreateItemWithTypesAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var withTypesRequest = new CreateItemWithTypesRequest(Guid.NewGuid(), item);
            try
            {
                await _apiClient.CreateItemWithTypesAsync(withTypesRequest);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateItemWithTypesAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Creating item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateItemWithTypesAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while creating item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task CreateItemAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var request = new CreateItemRequest(Guid.NewGuid(), item);
            try
            {
                await _apiClient.CreateItemAsync(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateItemAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Creating item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateItemAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while creating item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task ModifyItemWithTypesAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var withTypesRequest = new ModifyItemWithTypesRequest(Guid.NewGuid(), item);
            try
            {
                await _apiClient.ModifyItemWithTypesAsync(withTypesRequest);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await ModifyItemWithTypesAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Modifying item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await ModifyItemWithTypesAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while modifying item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task ModifyItemAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var request = new ModifyItemRequest(Guid.NewGuid(), item);
            try
            {
                await _apiClient.ModifyItemAsync(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await ModifyItemAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Modifying item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await ModifyItemAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while modifying item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task MakeTemporaryItemPermanentAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var request = new MakeTemporaryItemPermanentRequest(item.Id, item.Name, item.Comment,
                item.QuantityType.Id, item.QuantityInPacket, item.QuantityInPacketType?.Id,
                item.ItemCategoryId!.Value, item.ManufacturerId, item.Availabilities);
            try
            {
                await _apiClient.MakeTemporaryItemPermanent(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await MakeTemporaryItemPermanentAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Creating item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await MakeTemporaryItemPermanentAsync(item, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while creating item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task DeleteItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction)
        {
            var request = new DeleteItemRequest(Guid.NewGuid(), itemId);
            try
            {
                await _apiClient.DeleteItemAsync(request);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await DeleteItemAsync(itemId, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Deleting item failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await DeleteItemAsync(itemId, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while deleting item", e.Message, fragment);
                return;
            }

            onSuccessAction();
        }

        public async Task<ItemCategory> CreateItemCategoryAsync(string name, IAsyncRetryFragmentCreator fragmentCreator)
        {
            try
            {
                return await _apiClient.CreateItemCategoryAsync(name);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateItemCategoryAsync(name, fragmentCreator));
                _notificationService.NotifyError("Creating item category failed", contract.Message, fragment);
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateItemCategoryAsync(name, fragmentCreator));
                _notificationService.NotifyError("Unknown error while creating item category", e.Message, fragment);
            }

            return null;
        }

        public async Task CreateManufacturerAsync(string name, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction)
        {
            try
            {
                await _apiClient.CreateManufacturerAsync(name);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();

                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateManufacturerAsync(name, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Creating manufacturer failed", contract.Message, fragment);
                return;
            }
            catch (Exception e)
            {
                var fragment = fragmentCreator.CreateAsyncRetryFragment(async () =>
                    await CreateManufacturerAsync(name, fragmentCreator, onSuccessAction));
                _notificationService.NotifyError("Unknown error while creating manufacturer", e.Message, fragment);
                return;
            }

            await onSuccessAction();
        }
    }
}