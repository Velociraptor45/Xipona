using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Service
{
    public class ItemPriceCalculationService : IItemPriceCalculationService
    {
        private readonly IApiClient apiClient;
        private List<QuantityType> quantityTypes;

        public ItemPriceCalculationService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task InitializeAsync()
        {
            quantityTypes = (await apiClient.GetAllQuantityTypesAsync()).ToList();
        }

        public float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity)
        {
            var type = quantityTypes.FirstOrDefault(type => type.Id == quantityTypeId);
            if (type == null)
                throw new InvalidOperationException($"Quantity type {quantityTypeId} not recognized.");

            float price = (quantity / type.QuantityNormalizer) * pricePerQuantity;

            return (float)Math.Floor(price * 100) / 100;
        }
    }
}