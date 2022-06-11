using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services
{
    public interface IItemPriceCalculationService
    {
        void Initialize(IEnumerable<QuantityType> quantityTypes);

        float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity);
    }
}