using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Manufacturers.Services
{
    public class ManufacturerApiService : IManufacturerApiService
    {
        private readonly IApiClient _client;
        private readonly IShoppingListNotificationService _notificationService;

        public ManufacturerApiService(IApiClient client, IShoppingListNotificationService notificationService)
        {
            _client = client;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ManufacturerSearchResult>> SearchAsync(string searchInput)
        {
            return await _client.GetManufacturerSearchResultsAsync(searchInput);
        }

        public async Task<Manufacturer> GetAsync(Guid manufacturerId)
        {
            try
            {
                return await _client.GetManufacturerByIdAsync(manufacturerId);
            }
            catch (ApiException e)
            {
                var contract = e.DeserializeContent<ErrorContract>();
                _notificationService.NotifyError("Loading manufacturer failed", contract.Message);
            }

            return null;
        }
    }
}