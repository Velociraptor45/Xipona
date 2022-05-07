using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Manufacturers.Services
{
    public class ManufacturerLoadingService : IManufacturerLoadingService
    {
        private readonly IApiClient _client;

        public ManufacturerLoadingService(IApiClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ManufacturerSearchResult>> SearchAsync(string searchInput)
        {
            return await _client.GetManufacturerSearchResultsAsync(searchInput);
        }

        public async Task<Manufacturer> GetAsync(Guid manufacturerId)
        {
            return await _client.GetManufacturerByIdAsync(manufacturerId);
        }
    }
}