using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;

public static class StoreItemAvailabilityExtensions
{
    public static StoreItemAvailabilityReadModel ToReadModel(this IStoreItemAvailability model, IStore store,
        IStoreSection section)
    {
        return new StoreItemAvailabilityReadModel(store.ToStoreItemStoreReadModel(), model.Price,
            section.ToReadModel());
    }
}