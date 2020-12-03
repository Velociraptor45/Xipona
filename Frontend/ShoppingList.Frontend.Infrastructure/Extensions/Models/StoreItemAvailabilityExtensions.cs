using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models
{
    public static class StoreItemAvailabilityExtensions
    {
        public static ItemAvailabilityContract ToItemAvailabilityContract(this StoreItemAvailability model)
        {
            return new ItemAvailabilityContract()
            {
                StoreId = model.StoreId,
                Price = model.PricePerQuantity
            };
        }
    }
}