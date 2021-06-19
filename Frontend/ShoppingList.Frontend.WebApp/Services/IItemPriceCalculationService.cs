using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services
{
    public interface IItemPriceCalculationService
    {
        float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity);
        Task InitializeAsync();
    }
}