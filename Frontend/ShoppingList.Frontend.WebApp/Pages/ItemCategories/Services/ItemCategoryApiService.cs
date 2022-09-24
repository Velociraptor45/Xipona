using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ItemCategories;
using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.ItemCategories.Services
{
    public class ItemCategoryApiService : IItemCategoryApiService
    {
        private readonly IApiClient _client;
        private readonly IShoppingListNotificationService _notificationService;

        public ItemCategoryApiService(IApiClient client, IShoppingListNotificationService notificationService)
        {
            _client = client;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ItemCategorySearchResult>> SearchAsync(string searchInput)
        {
            return await _client.GetItemCategoriesSearchResultsAsync(searchInput);
        }

        public async Task<ItemCategory> GetAsync(Guid itemCategoryId)
        {
            try
            {
                return await _client.GetItemCategoryByIdAsync(itemCategoryId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();
                _notificationService.NotifyError("Loading item category failed", contract.Message);
            }

            return null;
        }

        public async Task<bool> DeleteAsync(Guid itemCategoryId)
        {
            try
            {
                await _client.DeleteItemCategoryAsync(itemCategoryId);
                return true;
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();
                _notificationService.NotifyError("Deleting item category failed", contract.Message);
            }

            return false;
        }

        public async Task<ItemCategory> CreateAsync(ItemCategory itemCategory)
        {
            return await CreateAsync(itemCategory.Name);
        }

        public async Task<ItemCategory> CreateAsync(string itemCategoryName)
        {
            try
            {
                return await _client.CreateItemCategoryAsync(itemCategoryName);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();
                _notificationService.NotifyError("Creating item category failed", contract.Message);
            }

            return null;
        }

        public async Task<bool> ModifyAsync(ItemCategory itemCategory)
        {
            var request = new ModifyItemCategoryRequest(itemCategory.Id, itemCategory.Name);

            try
            {
                await _client.ModifyItemCategoryAsync(request);
                return true;
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();
                _notificationService.NotifyError("Modifying item category failed", contract.Message);
            }

            return false;
        }
    }
}