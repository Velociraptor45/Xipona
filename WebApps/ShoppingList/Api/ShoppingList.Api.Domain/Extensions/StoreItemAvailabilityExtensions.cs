using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class StoreItemAvailabilityExtensions
    {
        public static StoreItemAvailabilityReadModel ToReadModel(this StoreItemAvailability model)
        {
            return new StoreItemAvailabilityReadModel(model.StoreId, model.Price);
        }
    }
}