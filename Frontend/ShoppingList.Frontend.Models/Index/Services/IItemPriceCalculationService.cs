using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index.Services
{
    public interface IItemPriceCalculationService
    {
        float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity);
        Task InitializeAsync();
    }
}