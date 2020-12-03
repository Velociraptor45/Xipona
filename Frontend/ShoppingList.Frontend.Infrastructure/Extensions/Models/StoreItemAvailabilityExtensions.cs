using ShoppingList.Api.Contracts.Commands.SharedContracts;
using ShoppingList.Frontend.Models.Items;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Models
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