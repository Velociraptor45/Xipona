using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services
{
    public class ItemPriceCalculationService : IItemPriceCalculationService
    {
        private List<QuantityType> _quantityTypes;

        public void Initialize(IEnumerable<QuantityType> quantityTypes)
        {
            _quantityTypes = quantityTypes.ToList();
        }

        public float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity)
        {
            var type = _quantityTypes.FirstOrDefault(type => type.Id == quantityTypeId);
            if (type == null)
                throw new InvalidOperationException($"Quantity type {quantityTypeId} not recognized.");

            float price = (quantity / type.QuantityNormalizer) * pricePerQuantity;

            return (float)Math.Round(price * 100, MidpointRounding.AwayFromZero) / 100;
        }
    }
}