using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemStoreExtensions
    {
        public static StoreItemStoreReadModel ToReadModel(this IStoreItemStore model)
        {
            return new StoreItemStoreReadModel(
                model.Id,
                model.Name,
                model.Sections.Select(s => s.ToReadModel()));
        }
    }
}