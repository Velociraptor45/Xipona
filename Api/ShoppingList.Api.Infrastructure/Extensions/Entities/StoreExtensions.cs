using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreExtensions
    {
        public static IStore ToDomain(this Infrastructure.Entities.Store entity)
        {
            return new Store(
                new StoreId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}