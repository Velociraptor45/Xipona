using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using RestEase;
using ShoppingList.Frontend.Redux.Shared.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services
{
    public class ItemsApiService : IItemsApiService
    {
        private readonly IApiClient _apiClient;
        private readonly IShoppingListNotificationService _notificationService;

        public ItemsApiService(IApiClient apiClient, IShoppingListNotificationService notificationService)
        {
            _apiClient = apiClient;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemsByItemCategoryAsync(
            Guid itemCategoryId)
        {
            try
            {
                return await _apiClient.SearchItemByItemCategoryAsync(itemCategoryId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();
                _notificationService.NotifyError("Searching for items by item category failed", contract.Message);
            }
            catch (Exception e)
            {
                _notificationService.NotifyError("Unknown error while searching for items by item category", e.Message);
            }

            return Enumerable.Empty<SearchItemByItemCategoryResult>();
        }
    }
}