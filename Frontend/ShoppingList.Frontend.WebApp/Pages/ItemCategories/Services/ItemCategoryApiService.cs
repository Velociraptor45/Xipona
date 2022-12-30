using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using RestEase;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Shared.Ports;
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
            return await _client.GetItemCategorySearchResultsAsync(searchInput);
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
    }
}