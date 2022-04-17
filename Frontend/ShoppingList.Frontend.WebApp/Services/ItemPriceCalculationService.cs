using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services
{
    public class ItemPriceCalculationService : IItemPriceCalculationService
    {
        private readonly IApiClient _apiClient;
        private List<QuantityType> _quantityTypes;

        public ItemPriceCalculationService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task InitializeAsync()
        {
            _quantityTypes = (await _apiClient.GetAllQuantityTypesAsync()).ToList();
        }

        public float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity)
        {
            var type = _quantityTypes.FirstOrDefault(type => type.Id == quantityTypeId);
            if (type == null)
                throw new InvalidOperationException($"Quantity type {quantityTypeId} not recognized.");

            float price = (quantity / type.QuantityNormalizer) * pricePerQuantity;

            return (float)Math.Floor(price * 100) / 100;
        }
    }
}